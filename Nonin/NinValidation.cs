using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonin
{
    public enum NinValidation
    {
        /// <summary>
        /// Used when InvalidNinException is deserialized and the reason is missing
        /// </summary>
        UnspecifiedReason,
        /// <summary>
        /// The specified number is too short
        /// </summary>
        TooShort,
        /// <summary>
        /// The specified number is too long
        /// </summary>
        TooLong,
        /// <summary>
        /// The string contains a non-digit character
        /// </summary>
        InvalidCharacter,
        /// <summary>
        /// The date of birth is not a valid date inside the valid range for identity numbers
        /// </summary>
        InvalidDateOfBirth,
        /// <summary>
        /// One or both of the control digits did not validate.        
        /// </summary>
        InvalidControlDigit,        
        /// <summary>
        /// The three 'personal digits' do not match the year in the date of birth, according to the 'century rules' of identity numbers. 
        /// </summary>
        InvalidCenturyCode,
        /// <summary>
        /// The number is valid, but is of a kind disallowed when validating/constructing the Nin.
        /// </summary>
        /// <example>
        /// <code>
        ///     Nin.Validate(aValidDNumber, NinKind.BirthNumber); //returns NinValidation.UnsupportedKind
        /// </code>
        /// </example>
        UnsupportedKind,
        /// <summary>
        /// The number is valid.
        /// </summary>
        Valid = int.MaxValue
    }
}
