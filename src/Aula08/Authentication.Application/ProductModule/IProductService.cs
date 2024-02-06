using Authentication.Domain.Repositories;

namespace Authentication.Application.ProductModule
{
    public interface IProductService
    {
        Task<IEnumerable<ProductModel>> GetAll();
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductModel>> GetAll()
        {
            //return await _productRepository.GetProductsAsync();
            return null;
        }
    }

}
