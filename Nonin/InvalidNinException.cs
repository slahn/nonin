using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace Nonin
{
    [Serializable]
    public sealed class InvalidNinException : Exception
    {
        public NinValidation Reason { get; private set; }
        public string Nin { get; private set; }

        public InvalidNinException()
            : this(string.Empty, NinValidation.UnspecifiedReason) { }

        public InvalidNinException(string message)
            : this(string.Empty, NinValidation.UnspecifiedReason, message) { }

        public InvalidNinException(string message, Exception innerException)
            : this(string.Empty, NinValidation.UnspecifiedReason, message, innerException) { }

        public InvalidNinException(string nin, NinValidation invalidReason)
            : this(nin, invalidReason, string.Format("Invalid NIN '{0}': {1}", nin, invalidReason.ToString())) { }

        public InvalidNinException(string nin, NinValidation invalidReason, string message)
            : this(nin, invalidReason, message, null)
        {
        }

        public InvalidNinException(string nin, NinValidation invalidReason, string message, Exception innerException)
            : base(message, innerException)
        {
            Nin = nin;
            Reason = invalidReason;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        private InvalidNinException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Nin = info.GetString("Nin");
            NinValidation parsedReason = NinValidation.UnspecifiedReason;
            if (Enum.TryParse(info.GetString("Reason"), out parsedReason))
            {
                Reason = parsedReason;
            }
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException("info");
            
            info.AddValue("Nin", Nin);
            info.AddValue("Reason", Reason.ToString());

            base.GetObjectData(info, context);
        }

    }
}
