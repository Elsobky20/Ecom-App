﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.core.Entites.Product
{
   public class Photo :BaseEntity<int>
    {
        public string ImageName { get; set; }
        public int ProductId { get; set; }
        //[ForeignKey("ProductId")]
        //public Product Product { get; set; }

    }
}
