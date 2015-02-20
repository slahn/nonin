using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonin
{
    public enum NinValidation
    {
        UnspecifiedReason,
        TooShort,
        TooLong,

        InvalidCharacter,
        InvalidDateOfBirth,
        InvalidControlDigit,        
        InvalidCenturyCode,

        UnsupportedKind,

        Valid = int.MaxValue
    }
}
