using Authentication.Domain.Entities;

namespace Authentication.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductAsync(Guid id);

    }
}
