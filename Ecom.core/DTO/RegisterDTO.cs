﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.core.DTO
{
    public record RegisterDTO
    {

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


    }
}
