namespace EAnalytics.Common.Exceptions
{
    public class ConnectionRefusedException : Exception
    {
        public ConnectionRefusedException(string message) : base(message)
        {
        }
    }
}