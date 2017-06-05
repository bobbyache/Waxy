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

    [Serializable()]
    public class BadRequestUriPatternException : Exception, ISerializable
    {
        public BadRequestUriPatternException() { }
        public BadRequestUriPatternException(string message) : base(message) { }
        public BadRequestUriPatternException(string message, Exception inner) : base(message, inner) { }
        public BadRequestUriPatternException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable()]
    public class BadRequestMethodLineException : Exception, ISerializable
    {
        public BadRequestMethodLineException() { }
        public BadRequestMethodLineException(string message) : base(message) { }
        public BadRequestMethodLineException(string message, Exception inner) : base(message, inner) { }
        public BadRequestMethodLineException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable()]
    public class BadImageUriException : Exception, ISerializable
    {
        public BadImageUriException() { }
        public BadImageUriException(string message) : base(message) { }
        public BadImageUriException(string message, Exception inner) : base(message, inner) { }
        public BadImageUriException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
