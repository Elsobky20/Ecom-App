using Ecom.core.DTO;
using Ecom.core.Entites;
using Ecom.core.Interfaces;
using Ecom.core.Services;
using Ecom.core.Sharing;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositories
{
    class AuthRepository:IAuth
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IEmailService emailService;

        public AuthRepository(UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.signInManager = signInManager;
        }
        public async Task<string> RegisterAsync(RegisterDTO registerDTO)
        {
            if (registerDTO == null)
            {
                throw null;
            }
            if (await userManager.FindByNameAsync(registerDTO.UserName) is not null)
            {
                return " this userName is already Exist";

            }
            if (await userManager.FindByEmailAsync(registerDTO.Email) is not null)
            {
                return " this Email is already Exist";
            }
            AppUser appUser = new AppUser
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email
            };
            var result = await userManager.CreateAsync(appUser, registerDTO.Password);
            if (result.Succeeded is not true)
            {
                return result.Errors.ToList().FirstOrDefault()?.Description ?? "Registration failed";
            }
            // send Active Email
            string code = await userManager.GenerateEmailConfirmationTokenAsync(appUser);
            await  SendEmail(appUser.Email, code, "Active", "ActiveEmail", "Please Active your account");

            return "Registration successful";

        }   
        public async Task SendEmail( string email , string code , string component , string subject ,string message)
        {
            var result = new EmailDTO(email, "ahmedelsobky630@gmail.com", subject , EmailStringBody.Send(email ,code ,component , message));
            await emailService.SendEmail(result);
        }
    }
}
