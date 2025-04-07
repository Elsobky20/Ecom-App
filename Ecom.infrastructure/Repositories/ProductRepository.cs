using AutoMapper;
using Ecom.core.DTO;
using Ecom.core.Entites.Product;
using Ecom.core.Interfaces;
using Ecom.core.Services;
using Ecom.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly IImageManagemntServce imageManagemntServce;

        public ProductRepository(AppDbContext context , IMapper mapper , IImageManagemntServce imageManagemntServce) : base(context)
        {
            this.context = context;
            this.mapper = mapper;
            this.imageManagemntServce = imageManagemntServce;
        }

        public async Task<bool> AddAsync(AddProductDTO productDTO)
        {
            if (productDTO == null) return false;
            var product = mapper.Map<Product>(productDTO);
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            var ImagePath = await imageManagemntServce.AddImageAsync(productDTO.Photos, productDTO.Name);
            var photos = ImagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = product.Id
            }).ToList();
            await context.Photos.AddRangeAsync(photos);
            await context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> UpdateAsync(UpdateProductDTO productDTO)
        {
            if (productDTO is null) return false;
            var FindedProduct = context.Products.Include(x=>x.Category)
                                                       .Include(x => x.Photos)
                                                       .FirstOrDefault(x => x.Id == productDTO.Id);
            if(FindedProduct is null) return false;

            mapper.Map(productDTO, FindedProduct);

            var FindPhoto = await context.Photos.Where(x => x.Id == productDTO.Id).ToListAsync();
            foreach (var photo in FindPhoto) 
            {
                imageManagemntServce.DeleteImageAsync(photo.ImageName);
            }
            context.Photos.RemoveRange(FindPhoto);

            var ImagePath = await imageManagemntServce.AddImageAsync(productDTO.Photos, productDTO.Name);

            var photos = ImagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = productDTO.Id
            }).ToList();
            await context.Photos.AddRangeAsync(photos);
            await context.SaveChangesAsync();
            return true;
        }


        public async Task DeleteAsync(Product product)
        {
            var photos = await context.Photos.Where(x => x.ProductId == product.Id).ToListAsync();
            foreach (var photo in photos)
            {
                imageManagemntServce.DeleteImageAsync(photo.ImageName);
            }
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }

    }
}
