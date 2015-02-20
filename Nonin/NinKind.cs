using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonin
{    
    /// <summary>
    /// The different kinds of NIN.
    /// - Birth number is assigned at birth or when granting citizenship
    /// - D-number is assigned to foreign workers when they get a work permit
    /// - H-number is assigned to non-citizens without a D-number that are treated by a hospital or other health institution. Unique to the health institution.
    ///   The birth date in a H-number might be fictional.
    /// - FH-number is a cross-institution identifier for non-citizens without a D-number. 
    ///   a FH-number does not contain information about birth date or sex.
    /// </summary>
    public enum NinKind
    {   
        BirthNumber,        
        DNumber,        
        HNumber,
        FHNumber
    }
}
