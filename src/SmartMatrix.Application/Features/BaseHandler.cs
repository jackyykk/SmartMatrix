namespace SmartMatrix.Application.Features
{
    public class BaseHandler
    {
        protected string GetErrorMessage(Exception ex)
        {
            string message = ex.Message + (ex.InnerException != null ? " || " + ex.InnerException.Message : "");
            return message;
        }
    }
}