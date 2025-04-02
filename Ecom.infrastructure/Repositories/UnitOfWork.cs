using Ecom.core.Interfaces;
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

        public UnitOfWork(AppDbContext context)
        {
            this.context = context;
            categoryRepository = new CategoryRepository(context);
            productRepository = new ProductRepository(context);
            photoRepository = new PhotoRepository(context);
        }
        public ICategoryRepository categoryRepository { get; }


        public IProductRepository productRepository { get; }

        public IPhotoRepository photoRepository { get; }
    }
}
