using Microsoft.AspNetCore.Mvc;
using Authentication.Application.ProductModule;

namespace Authentication.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/<ProductController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductModel>))]
        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            return await _productService.GetAll();
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductModel))]
        public ProductModel Get(int id)
        {
            /*var product = _productService.Get(id);
            if (product == null)
                return null;

            return product;*/
            return new ProductModel();
        }

        // POST api/<ProductController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
