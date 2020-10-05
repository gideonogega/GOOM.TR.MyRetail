using GOOM.TR.MyRetail.NET.Models;
using GOOM.TR.MyRetail.NET.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GOOM.TR.MyRetail.NET.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet("{id}")]
        public async Task<Product> Get([FromRoute] long id)
        {
            return await productService.GetProductAsync(id);
        }

        [HttpPut("{id}/current_price")]
        public async Task<Price> SetCurrentPrice([FromRoute] long id, [FromBody] Price currentPrice)
        {
            return await productService.SetProductCurrentPriceAsync(id, currentPrice);
        }

        [HttpDelete("{id}/current_price")]
        public async Task<bool> DeleteCurrentPrice([FromRoute] long id)
        {
            return await productService.DeleteProductCurrentPriceAsync(id);
        }
    }
}
