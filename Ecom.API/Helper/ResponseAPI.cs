namespace Ecom.API.Helper
{
    public class ResponseAPI
    {
        public ResponseAPI(int statuscode ,string message=null)
        {
            StatusCode = statuscode;
            Message = message ?? GetmessageFromStatusCode(StatusCode);
        }
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        private string GetmessageFromStatusCode(int statusCode)
        {
            return statusCode switch
            {
                200 => "Success",
                201 => "Created",
                204 => "No Content",
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Not Found",
                500 => "Internal Server Error",
                _ => "Unknown Status Code"
            };
        }
    }
}
