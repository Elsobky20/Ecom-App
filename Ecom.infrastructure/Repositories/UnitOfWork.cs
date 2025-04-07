using AutoMapper;
using Ecom.core.Interfaces;
using Ecom.core.Services;
using Ecom.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly IImageManagemntServce imageManagemntServce;

        public UnitOfWork(AppDbContext context, IMapper mapper, IImageManagemntServce imageManagemntServce)
        {
            this.context = context;
            this.mapper = mapper;
            this.imageManagemntServce = imageManagemntServce;
            categoryRepository = new CategoryRepository(this.context);
            productRepository = new ProductRepository(this.context,this.mapper , this.imageManagemntServce);
            photoRepository = new PhotoRepository(this.context);
           
        }
        public ICategoryRepository categoryRepository { get; }


        public IProductRepository productRepository { get; }

        public IPhotoRepository photoRepository { get; }
    }
}
