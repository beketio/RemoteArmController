using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmController.Core
{
    [Serializable]
    public class ComponentException: Exception
    {
        private ComponentException()
        {
        }

        public ComponentException(string message) : base(message)
        {

        }

        public ComponentException(Exception innerException) : base(innerException?.Message, innerException)
        {

        }

        public ComponentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ComponentException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
            throw new NotImplementedException();
        }
    }
}
