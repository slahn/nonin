using System;
using System.Collections.Generic;
using System.Linq;

namespace Nonin
{
    public sealed partial class Nin : IComparable<Nin>, IComparable
    {
        public override int GetHashCode()
        {
            //we use only 9 bytes for the hash. The two control digits don't add any entropy.

            //All bytes are only values 0-9, so we actually only use 5 bits from each byte.
            //This isn't really accounted for in the mix algorithm below, but that doesn't seem to matter too much in practice.

            var digits = DigitStringToByteArray(ToString());

            uint last = digits[8];
            uint first = digits[0];

            //as the starting value, distribute the high/low bytes through the whole uint.
            uint hash = (last << 27) | (first << 22) | (last << 17) | (first << 12) | (last << 7) | (first << 2) | last;

            uint a = 0x9e3779b9;
            uint b = 0x9e3779b9;


            a += ((uint)digits[7]) << 24;
            a += ((uint)digits[6]) << 16;
            a += ((uint)digits[5]) << 8;
            a += ((uint)digits[4]);

            b += ((uint)digits[3]) << 24;
            b += ((uint)digits[2]) << 16;
            b += ((uint)digits[1]) << 8;
            b += ((uint)digits[0]);

            // mix a, b and dist
            var c = hash;
            a -= b; a -= c; a ^= (c >> 13);
            b -= c; b -= a; b ^= (a << 8);
            c -= a; c -= b; c ^= (b >> 13);
            a -= b; a -= c; a ^= (c >> 12);
            b -= c; b -= a; b ^= (a << 16);
            c -= a; c -= b; c ^= (b >> 5);
            a -= b; a -= c; a ^= (c >> 3);
            b -= c; b -= a; b ^= (a << 10);
            c -= a; c -= b; c ^= (b >> 15);

            return unchecked((int)c);
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, null)) return false;

            if (obj is Nin)
            {

                if (Object.ReferenceEquals(this, obj))
                {
                    return true;
                }
                else
                {
                    return this.ToString().Equals(obj.ToString());
                }
            }

            return false;
        }

        public static bool operator ==(Nin one, Nin two)
        {
            if (Object.ReferenceEquals(one, two))
            {
                return true;
            }
            if (Object.ReferenceEquals(one, null) || Object.ReferenceEquals(two, null))
            {
                return false;
            }

            return one.Equals(two);
        }

        public static bool operator !=(Nin one, Nin two)
        {
            return !(one == two);
        }


        public static bool operator <(Nin left, Nin right)
        {
            return Compare(left, right) < 0;
        }

        public static bool operator <=(Nin left, Nin right)
        {
            return Compare(left, right) <= 0;
        }

        public static bool operator >(Nin left, Nin right)
        {
            return Compare(left, right) > 0;
        }

        public static bool operator >=(Nin left, Nin right)
        {
            return Compare(left, right) >= 0;
        }

        /// <summary>
        /// Simple lexographic sort order.
        /// </summary>
        public int CompareTo(Nin other)
        {
            //Any object is greater than null
            if (object.ReferenceEquals(other, null)) return 1;

            //Simple lexographic sort.
            return _value.CompareTo(other._value);
        }

        public static int Compare(Nin left, Nin right)
        {
            //same object, or both null
            if (object.ReferenceEquals(left, right)) return 0;
            //only left is null
            if (object.ReferenceEquals(left, null)) return -1;
            //left is not null - we can call CompareTo
            return left.CompareTo(right);
        }

        /// <summary>
        /// Sort order is decided first by date of birth, and then by the personal digits.
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown if obj is not of correct type.</exception>
        public int CompareTo(object obj)
        {
            //Any object is greater than null -- cant' check type of null, assume its correct (consistent with CLR types).
            if (object.ReferenceEquals(obj, null)) return 1;

            if (!(obj is Nin))
            {
                throw new ArgumentException("Object must be of type "+GetType().FullName, "obj");
            }

            return CompareTo((Nin)obj);
        }
    }
}
