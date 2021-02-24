namespace AspDotnetCoreApi.Exceptions {
    public class IncorrectPasswordException : System.Exception
    {
        public IncorrectPasswordException() { }
        public IncorrectPasswordException(string message) : base(message) { }
        public IncorrectPasswordException(string message, System.Exception inner) : base(message, inner) { }
        protected IncorrectPasswordException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}