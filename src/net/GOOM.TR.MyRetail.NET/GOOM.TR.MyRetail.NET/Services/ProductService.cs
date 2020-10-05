using GOOM.TR.MyRetail.NET.Models;
using GOOM.TR.MyRetail.NET.Repos;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GOOM.TR.MyRetail.NET.Services
{
    public interface IProductService
    {
        Task<Product> GetProductAsync(long id);
        Task<Price> SetProductCurrentPriceAsync(long id, Price price);
        Task<bool> DeleteProductCurrentPriceAsync(long id);
    }

    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _log;
        private readonly IProductRepo _productRepo;
        private readonly IPriceRepo _priceRepo;

        public ProductService(ILogger<ProductService> log, IProductRepo productRepo, IPriceRepo priceRepo)
        {
            _log = log;
            _productRepo = productRepo;
            _priceRepo = priceRepo;
        }

        public async Task<Product> GetProductAsync(long id)
        {
            try
            {
                var product = await _productRepo.GetProductAsync(id);
                if (product == null) return product;

                product.CurrentPrice = await _priceRepo.GetPriceByProductIdAsync(id);
                return product;
            } 
            catch (Exception ex)
            {
                _log.LogError(ex, ex.Message);
            }
            return null;
        }

        public async Task<Price> SetProductCurrentPriceAsync(long id, Price price)
        {
            try
            {
                return await _priceRepo.SetProductPriceAsync(id, price);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message, ex);
            }
            return null;
        }

        public async Task<bool> DeleteProductCurrentPriceAsync(long id)
        {
            try
            {
                return await _priceRepo.DeleteProductPriceAsync(id);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message, ex);
            }
            return false;
        }
    }
}
