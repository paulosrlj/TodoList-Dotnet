namespace TodoList.Exceptions
{
    public class DoesntExistsException : Exception
    {
        public DoesntExistsException()
        {
        }

        public DoesntExistsException(string message)
            : base(message)
        {
        }

        public DoesntExistsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
