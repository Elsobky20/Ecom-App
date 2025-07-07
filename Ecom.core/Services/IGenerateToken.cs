using Ecom.core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.core.Services
{
    public interface IGenerateToken
    {
        string GetAndGenerateToken(AppUser user);
    }
}
