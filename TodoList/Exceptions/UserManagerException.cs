namespace TodoList.Exceptions
{
    public class UserManagerException : Exception
    {
        public UserManagerException(IEnumerable<string> errors) : base(string.Join(", ", errors))
        {

        }
    }
}
