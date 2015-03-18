using System;
using System.Collections.Generic;
using System.Linq;

namespace Nonin
{

    /// <summary>
    /// Represents a norwegian National Identity Number (fødselsnummer).    
    /// </summary>
    public sealed partial class Nin : IComparable<Nin>, IComparable
    {

        private const int LENGTH = 11;
        private const int GENDER_POSITION = 8;

        //The two checksum series
        //for digit 10, the first check digit
        private static int[] checksumSeries1 = new[] {3, 7, 6, 1, 8, 9, 4, 5, 2, 1};
	    //for digit 11, the 2nd check digit
	    private static int[] checksumSeries2 = new[] {5, 4, 3, 2, 7, 6, 5, 4, 3, 2, 1};

        
        
        // The actual value of the nin.
        // We _know_ that this is a string of digits, 11 characters long.
        private readonly string _value;
        /// <summary>
        /// The individual digits of the Nin.
        /// </summary>
        public IEnumerable<byte> Digits { get { return DigitStringToByteArray(_value); } }

        /// <summary>
        /// The gender encoded in the Nin.
        /// Is unspecified for FH numbers.
        /// </summary>
        public Gender Gender
        {
            get
            {
                if (Kind == NinKind.FHNumber) return Gender.Unspecified;
                
                var indicator = _value[GENDER_POSITION] - '0';
                return (indicator % 2) == 0 ? Gender.Female : Gender.Male;
            }
        }


        // These two properties could be generated on the fly, but _must_ be generated when validating a number, so we cache them.

        /// <summary>
        /// Specifies the kind of Nin.
        /// </summary>
        public NinKind Kind { get; private set; }

        /// <summary>
        /// The date of birth, at midnight in UTC time.   
        /// Is null for FH numbers.
        /// </summary>
        public DateTime? DateOfBirth { get; private set; }

                
        /// <summary>
        /// Creates a Nin instance using the specified string as a source.
        /// </summary>
        /// <exception cref="InvalidNinException">If the supplied string is not a valid Nin.</exception>
        public Nin(string input)
        {
            DateTime? dob;
            NinKind typ;

            var isValid = Validate(input, out typ, out dob);
            if (isValid != NinValidation.Valid)
            {
                throw new InvalidNinException(input, isValid);
            }
            
            _value = input;    
            DateOfBirth = dob;
            Kind = typ;
        }

        /// <summary>
        /// Creates a Nin instance using the specified number as a source.
        /// </summary>
        /// <exception cref="InvalidNinException">If the supplied number is not a valid Nin.</exception>
        public Nin(long ninNumber) : this(ninNumber.ToString("D11")) { }


        private Nin(string alreadyValidatedValue, DateTime? dob, NinKind typ)
        {
            _value = alreadyValidatedValue;
            DateOfBirth = dob;
            Kind = typ;
        }

        /// <summary>
        /// Get the numeric value of the nin.
        /// This number will not fit in 32 bits, and must be represented as a long.
        /// </summary>        
        public long ToNumber()
        {
            return long.Parse(ToString());
        }

        /// <summary>
        /// Gets the string representation of the nin.
        /// </summary>        
        public override string ToString()
        {
            return _value;
        }


        /// <summary>
        /// Try to create a Nin from a string input.
        /// </summary>        
        public static bool TryParse(string nin, out Nin result)
        {
            
            DateTime? dob;
            NinKind typ;
            var validationResult = Validate(nin, out typ, out dob);

            if (validationResult == NinValidation.Valid)
            {
                result = new Nin(nin, dob, typ);
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }
                
        /// <summary>
        /// Check if the string is a valid nin.
        /// </summary>        
        public static bool IsValid(string nin)
        {
            return Validate(nin) == NinValidation.Valid;
        }

        /// <summary>
        /// Check if the string is a valid nin of one of the specified kinds.
        /// </summary> 
        public static bool IsValid(string nin, params NinKind[] allowedKinds)
        {
            Nin n;
            if (!Nin.TryParse(nin, out n))
            {
                return false;
            }

            return allowedKinds.Contains(n.Kind);
        }
        
        /// <summary>
        /// Check if the string is a valid nin.
        /// </summary>        
        /// <returns>A value indicating how the validation failed, or NinValidation.Valid.</returns>
        public static NinValidation Validate(string nin)
        {
            NinKind t;
            DateTime? dob;
            return Validate(nin, out t, out dob);
        }
        
        private static NinValidation Validate(string nin, out NinKind t, out DateTime? dob)
        {
            t = NinKind.BirthNumber;
            dob = DateTime.MinValue;
            
            // Basic sanity tests -- null check, length and allowed characters
            if (nin == null || nin.Length < LENGTH)
            {
                return NinValidation.TooShort;
            }
            else if (nin.Length > LENGTH)
            {
                return NinValidation.TooLong;
            }

            if (!nin.All(Char.IsDigit))
            {
                return NinValidation.InvalidCharacter;
            }

            var digits = DigitStringToByteArray(nin);

            // Check the control digits
            if(
                !IsChecksumValid(checksumSeries1, digits)
                ||
                !IsChecksumValid(checksumSeries2, digits))
            {
                return NinValidation.InvalidControlDigit;
            }

            // Extract birth date
            var day = int.Parse(nin.Substring(0, 2));
            var month = int.Parse(nin.Substring(2, 2));
            var year = int.Parse(nin.Substring(4, 2));

            // FH numbers start with 8/9
            if (digits[0] > 7)
            {
                t = NinKind.FHNumber;
                dob = null;
                return NinValidation.Valid;
            }
            else
            {
                // Century is encoded in the three personal digits following the date.
                var personalDigits = int.Parse(nin.Substring(6, 3));
                var fullYear = FindYearWithCentury(personalDigits, year);
                if (!fullYear.HasValue)
                {
                    return NinValidation.InvalidCenturyCode;
                }
                year = fullYear.Value;

                // D-numbers and H-numbers are encoded in the day/month field


                if (day > 40)
                {
                    t = NinKind.DNumber;
                    day -= 40;
                }
                else if (month > 40)
                {
                    t = NinKind.HNumber;
                    month -= 40;
                }


                if (
                    year < 1854 ||
                    year > 2039 ||
                    month < 1 || month > 12 ||
                    day < 1 || day > DateTime.DaysInMonth(year, month))
                {
                    return NinValidation.InvalidDateOfBirth;
                }

                dob = new DateTime(year, month, day, 0, 0, 0, 0, DateTimeKind.Utc);
                return NinValidation.Valid;
            }            
        }

        /// <summary>
        /// Validate that a number matches the control sequence.
        /// Each number in the control sequence is multiplied by the digit in the matching position in the input.
        /// Digits in the input past the length of the control sequence are ignored.
        /// The products are added together. If the result % 11 is 0, then the number is valid.
        /// </summary>
        private static bool IsChecksumValid(int[] series, byte[] numberToCheck)
        {
            long sum = 0;

            for (var i = 0; i < series.Length; ++i)
            {
                sum += numberToCheck[i] * series[i];
            }

            return sum % 11 == 0;
        }

        /// <summary>
        /// Find the full year based on the 3-digit 'personal number' (pos. 7,8,9 in the full number), and the two-digit year.
        /// </summary>
        private static int? FindYearWithCentury(int personalNumber, int year)
        {
            //Connection between year and 3 last digits in number
            //a) … 1854-1899, uses 749-500,
            //b) … 1900-1999, uses 499-000,
            //c) … 1940-1999, also uses 999-900
            //d) … 2000-2039, uses 999-500.
           

            //Rule A
            if (year >= 54 && personalNumber >= 500 && personalNumber < 750)
            {
                return 1800 + year;
            }
            // rule B
            else if (personalNumber < 500)
            {
                return 1900 + year;
            }
            // Rule C
            else if (personalNumber >= 900 && year >= 40)
            {
                return 1900 + year;
            }
            //Rule D
            else if (year < 40 && personalNumber >= 500)
            {
                return 2000 + year;
            }
            else
            {
                return null;
            }     
        }
             


        private static byte[] DigitStringToByteArray(string digits)
        {
            var b = new byte[digits.Length];
            for (var i = 0; i < digits.Length; ++i)
            {
                b[i] = (byte)(digits[i] - '0');
            }
            return b;
        }   
    }
}
