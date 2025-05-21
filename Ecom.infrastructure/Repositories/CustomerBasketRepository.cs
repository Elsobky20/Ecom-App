using Ecom.core.Entites;
using Ecom.core.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositories
{
    class CustomerBasketRepository : ICustomerBasketRepository
    {
        private readonly IDatabase database;
        public CustomerBasketRepository(IConnectionMultiplexer redis)
        {
            database = redis.GetDatabase();
        }
        public async Task<CustomerBasket> GetBasketAsync(string Id)
        {
            var result = await database.StringGetAsync(Id);
            if (!string.IsNullOrEmpty(result))
            {
                return JsonSerializer.Deserialize<CustomerBasket>(result);
            }
            return null;
        }

        public async Task<bool> DeleteBasketAsync(string Id)
        {
            return await database.KeyDeleteAsync(Id);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var _basket = await database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(3));
            if (_basket)
            {
                return await GetBasketAsync(basket.Id);
            }
            return null;
        }
    }
}
