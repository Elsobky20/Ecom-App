namespace Ecom.API.Helper
{
    public class APIExceptions : ResponseAPI
    {
        public APIExceptions(int statuscode, string message = null , string details = null) : base(statuscode, message)
        {
            Details = details;
        }
        public string Details { get; set; } = string.Empty;
    }
}
