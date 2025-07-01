using AutoMapper;
using Ecom.core.Entites;
using Ecom.core.Interfaces;
using Ecom.core.Services;
using Ecom.infrastructure.Data;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace Ecom.infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly IImageManagemntServce imageManagemntServce;
        private readonly IConnectionMultiplexer redis;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IEmailService emailService;


        public ICategoryRepository categoryRepository { get; }
        public IProductRepository productRepository { get; }
        public IPhotoRepository photoRepository { get; }
        public ICustomerBasketRepository CustomerBasket { get; }

        public IAuth Auth { get; }


        public UnitOfWork(AppDbContext context, IMapper mapper, IImageManagemntServce imageManagemntServce, IConnectionMultiplexer redis, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            this.context = context;
            this.mapper = mapper;
            this.imageManagemntServce = imageManagemntServce;
            this.redis = redis;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailService = emailService;
            categoryRepository = new CategoryRepository(this.context);
            productRepository = new ProductRepository(this.context, this.mapper, this.imageManagemntServce);
            photoRepository = new PhotoRepository(this.context);
            CustomerBasket = new CustomerBasketRepository(this.redis);
            Auth = new AuthRepository(userManager , emailService , signInManager);
            
        }

    }
}
