
namespace CID_Tester.Exceptions
{
    internal class TestParameterException : Exception
    {
        public TestParameterException(string msg) : base($"Test Parameter Exception: {msg}") { }
    }
}
