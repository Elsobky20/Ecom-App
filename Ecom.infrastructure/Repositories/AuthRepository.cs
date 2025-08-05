using Ecom.core.DTO;
using Ecom.core.Entites;
using Ecom.core.Interfaces;
using Ecom.core.Services;
using Ecom.core.Sharing;
using Ecom.infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Operators.Utilities;
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
        private readonly AppDbContext _context;

        public AuthRepository(UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signInManager, IGenerateToken token , AppDbContext _context)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.signInManager = signInManager;
            this.token = token;
            this._context = _context;
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

        public async Task<bool> UpdateAddress(string email, Address address)
        {
            var findUser = await userManager.FindByEmailAsync(email);
            if (findUser == null)
            {
                return false;
            }
            var myAddress = await _context.Addresses.FirstOrDefaultAsync(x => x.AppUserId == findUser.Id);
            if (myAddress == null)
            {
                address.AppUserId = findUser.Id;
                await _context.Addresses.AddAsync(address);
            }
            else 
            {
               
                myAddress.FirstName = address.FirstName;
                myAddress.LastName = address.LastName;
                myAddress.Street = address.Street;
                myAddress.City = address.City;
                myAddress.State = address.State;
                myAddress.ZipCode = address.ZipCode;
                 _context.Addresses.Update(myAddress);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ShipAddressDTO> GetUserAddress(string email)
        {

            var findUser = await userManager.FindByEmailAsync(email);
            var myAddress = await _context.Addresses.FirstOrDefaultAsync(x => x.AppUserId == findUser.Id);

            ShipAddressDTO shipAddressDTO = new ShipAddressDTO
            {
                FirstName = myAddress.FirstName,
                LastName = myAddress.LastName,
                Street = myAddress.Street,
                City = myAddress.City,
                State = myAddress.State,
                ZipCode = myAddress.ZipCode,
                
            };
            return shipAddressDTO;
        }
    }
}
