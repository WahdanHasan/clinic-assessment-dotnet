namespace clinic_assessment_redone.Middleware.ErrorHandler.Exceptions
{
    public class RestException : GenericException<string>
    {
        public RestException(int code, string message) : base(code, message)
        {
        }
    }
}
