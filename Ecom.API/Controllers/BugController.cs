using AutoMapper;
using Ecom.API.Helper;
using Ecom.core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{

    public class BugController : BaseController
    {
        public BugController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpGet("not-found")]
        public IActionResult NotFoundRequest()
        {
            return NotFound(new ResponseAPI(404));
        }
        [HttpGet("server-error")]
        public IActionResult ServerError()
        {
            return StatusCode(500, new ResponseAPI(500));
        }
        [HttpGet("bad-request/{Id}")]
        public IActionResult BadRequest(int Id)
        {
            return Ok();
        }
        [HttpGet("bad-request")]
        public IActionResult BadRequest() 
        {
            return BadRequest();
        }
    }
}
