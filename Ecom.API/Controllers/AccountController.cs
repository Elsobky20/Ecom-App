using AutoMapper;
using Ecom.API.Helper;
using Ecom.core.DTO;
using Ecom.core.Entites;
using Ecom.core.Interfaces;
using Ecom.infrastructure.Data.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
                return BadRequest(new ResponseAPI(400, result));
            }
            return Ok(new ResponseAPI(200, result));
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
                return BadRequest(new ResponseAPI(400, result));
            }
            Response.Cookies.Append("token", result, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                Domain = "localhost",
                Expires = DateTimeOffset.UtcNow.AddDays(1),
                IsEssential = true,
                SameSite = SameSiteMode.None
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
        public async Task<IActionResult> forget(string email)
        {
            var result = await work.Auth.SendEmailForForgotPasswordAsync(email);
            if (!result)
            {
                return BadRequest(new ResponseAPI(400, "Active Account Failed"));
            }
            return Ok(new ResponseAPI(200, "Active Account Successfully"));
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> reset(ResetPasswordDTO passwordDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await work.Auth.ResetPassword(passwordDTO);
                if (result != "Reset password successful")
                {
                    return BadRequest(new ResponseAPI(400, result));
                }
                return Ok(new ResponseAPI(200, result));
            }
            return BadRequest(new ResponseAPI(400, "Model is not Valid"));
        }

        [HttpPut("update-address")]
        public async Task<IActionResult> UpdateAddress(ShipAddressDTO addressDTO)
        {
            if (addressDTO == null)
            {
                return BadRequest(new ResponseAPI(400, "Address is null"));
            }
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var address = mapper.Map<Address>(addressDTO);
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new ResponseAPI(401, "User not authenticated"));
            }
            var result = await work.Auth.UpdateAddress(email, address);
            if (!result)
            {
                return BadRequest(new ResponseAPI(400, "Update Address Failed"));
            }
            return Ok(new ResponseAPI(200, "Update Address Successfully"));
        }

        [HttpGet("get-address-for-user")]
        public async Task<IActionResult> GetAddress()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new ResponseAPI(401, "User not authenticated"));
            }
            var address = await work.Auth.GetUserAddress(email);
            if (address == null)
            {
                return NotFound(new ResponseAPI(404, "Address not found"));
            }
            return Ok(address);
        }

    }
}
