using System;
using System.Runtime.Serialization;

namespace CygX1.Waxy.Http.Exceptions
{
    [Serializable()]
    public class InvalidHttpRequestHeader : Exception, ISerializable
    {
        public InvalidHttpRequestHeader() { }
        public InvalidHttpRequestHeader(string message) : base(message) { }
        public InvalidHttpRequestHeader(string message, Exception inner) : base(message, inner) { }
        public InvalidHttpRequestHeader(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
