namespace CID_Tester.Exceptions
{
    public class IncorrectLoginException : Exception
    {
        public IncorrectLoginException() : base("Incorrect username or password") { }
    }
}
