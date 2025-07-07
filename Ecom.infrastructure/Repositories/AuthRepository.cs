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
        private readonly IGenerateToken token;


        public AuthRepository(UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signInManager, IGenerateToken token)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.signInManager = signInManager;
            this.token = token;
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
                Email = registerDTO.Email ,
                DisaplayName = registerDTO.DisplayName,
            };
            var result = await userManager.CreateAsync(appUser, registerDTO.Password);
            if (result.Succeeded is not true)
            {
                return result.Errors.ToList().FirstOrDefault()?.Description ?? "Registration failed";
            }
            // send Active Email
            string code = await userManager.GenerateEmailConfirmationTokenAsync(appUser);
            await  SendEmail(appUser.Email, code, "active", "ActiveEmail", "Please Active your account");

            return "Done";

        }
        public async Task<string> LoginAsync(LoginDTO login)
        {
            if (login == null)
            {
                return null;
            }
            var findUser = await userManager.FindByEmailAsync(login.Email);

            if (!findUser.EmailConfirmed)
            {
                var token = await userManager.GenerateEmailConfirmationTokenAsync(findUser);
                await SendEmail(findUser.Email, token, "active", "ActiveEmail", "Please Active your account");
                return "Please Active your account, we send you email to active your account";
            }

            var result = await signInManager.CheckPasswordSignInAsync(findUser, login.Password, true); //lockoutOnFailure to prevent user if faild in 5 request
            if (result.Succeeded is not true)
            {
                return "Please confirm your Email and Password , something went wrong";
            }
            return token.GetAndGenerateToken(findUser);
        }
        public async Task SendEmail( string email , string code , string component , string subject ,string message)
        {
            var result = new EmailDTO(email, "ahmedelsobky630@gmail.com", subject , EmailStringBody.Send(email ,code ,component , message));
            await emailService.SendEmail(result);
        }
        public async Task<bool> SendEmailForForgotPasswordAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            await SendEmail(user.Email, token, "Reset-password", "Reset Password", "Please reset your password");
            return true;
        }
        public async Task<string> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            if (resetPasswordDTO == null)
            {
                return "Invalid request";
            }
            var user = await userManager.FindByEmailAsync(resetPasswordDTO.Email);
            if (user == null)
            {
                return "User not found";
            }
            var result = await userManager.ResetPasswordAsync(user, resetPasswordDTO.Token, resetPasswordDTO.Password);
            if (result.Succeeded is not true)
            {
                return result.Errors.ToList().FirstOrDefault()?.Description ?? "Reset password failed";
            }
            return "Reset password successful";
        }
        public async Task<bool> ActiveAccount(ActiveAccountDTO activeAccountDTO )
        {
            if (activeAccountDTO == null)
            {
                return false;
            }
            var user = await userManager.FindByEmailAsync(activeAccountDTO.Email);
            if (user == null)
            {
                return false;
            }
            var result = await userManager.ConfirmEmailAsync(user, activeAccountDTO.Token);
            if (result.Succeeded is not true)
            {
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                await SendEmail(user.Email, token, "active", "ActiveEmail", "Please Active your account");
                return false ;
            }
            return true;
        }
    }
}
