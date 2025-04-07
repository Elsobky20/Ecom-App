using Ecom.API.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("Errors/{statusCode}")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        public IActionResult HandleError(int statusCode)
        {
            var result = new ResponseAPI(statusCode);
            return new ObjectResult(result);
        }
    }
}
