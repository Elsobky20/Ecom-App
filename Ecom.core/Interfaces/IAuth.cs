using Ecom.core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.core.Interfaces
{
    public interface IAuth
    {
        Task<string> RegisterAsync(RegisterDTO registerDTO);
        Task<string> LoginAsync(LoginDTO login);
        Task SendEmail(string email, string code, string component, string subject, string message);
        Task<bool> SendEmailForForgotPasswordAsync(string email);
        Task<string> ResetPassword(ResetPasswordDTO resetPasswordDTO);
        Task<bool> ActiveAccount(ActiveAccountDTO activeAccountDTO);
    }
}
