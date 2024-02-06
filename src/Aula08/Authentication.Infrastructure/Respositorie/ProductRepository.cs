using Authentication.Domain.Entities;
using Authentication.Domain.Repositories;

namespace Authentication.Infrastructure.Respositorie
{
    public class ProductRepository : IProductRepository
    {
        public Task<Product> GetProductAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetProductsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
