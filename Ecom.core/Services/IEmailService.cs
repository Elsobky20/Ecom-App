using Ecom.core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.core.Services
{
    public interface IEmailService
    {
        Task SendEmail(EmailDTO emailDTO); 
    }
}
