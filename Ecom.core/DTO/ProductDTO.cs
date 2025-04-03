using AutoMapper;
using Ecom.core.Entites.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.core.DTO
{
    public record ProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public virtual List<PhotoDTO> Photos { get; set; } = new List<PhotoDTO>();
        public string CategoryName { get; set; }


    }
    public record PhotoDTO
    {
        public string ImageName { get; set; }
        public int ProductId { get; set; }

    }
}
