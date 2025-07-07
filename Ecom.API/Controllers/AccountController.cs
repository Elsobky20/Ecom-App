using AutoMapper;
using Ecom.API.Helper;
using Ecom.core.DTO;
using Ecom.core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{

    public class AccountController : BaseController
    {
        public AccountController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (registerDTO == null)
            {
                return BadRequest(new ResponseAPI(400));
            }
            var result = await work.Auth.RegisterAsync(registerDTO);
            if (result != "Done")
            {
                return BadRequest(new ResponseAPI(400 ,result));
            }
            return Ok(new ResponseAPI(200 ,result));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            if (login == null)
            {
                return BadRequest(new ResponseAPI(400));
            }
            var result = await work.Auth.LoginAsync(login);
            if (result == null || result.StartsWith("Please"))
            {
                return BadRequest(new ResponseAPI(400 ,result));
            }
            Response.Cookies.Append("token", result, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                Domain = "localhost",
                Expires = DateTimeOffset.UtcNow.AddDays(1) ,
                IsEssential = true,
                SameSite = SameSiteMode.Strict
            });
            return Ok(new ResponseAPI(200, result));
        }

        [HttpPost("active-account")]
        public async Task<IActionResult> Active(ActiveAccountDTO activeAccountDTO)
        {
            if (activeAccountDTO == null)
            {
                return BadRequest(new ResponseAPI(400));
            }
            var result = await work.Auth.ActiveAccount(activeAccountDTO);
            if (!result)
            {
                return BadRequest(new ResponseAPI(400, "Active Account Failed"));
            }
            return Ok(new ResponseAPI(200, "Active Account Successfully"));
        }

        [HttpGet("send-email-forget-password")]
        public async Task<IActionResult> forget( string email)
        {
            var result = await work.Auth.SendEmailForForgotPasswordAsync(email);
            if (!result)
            {
                return BadRequest(new ResponseAPI(400, "Active Account Failed"));
            }
            return Ok(new ResponseAPI(200, "Active Account Successfully"));
        }  
    }
}
