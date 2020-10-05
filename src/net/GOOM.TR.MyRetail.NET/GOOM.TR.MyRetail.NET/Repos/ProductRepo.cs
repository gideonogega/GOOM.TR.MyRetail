using GOOM.TR.MyRetail.NET.Models;
using Microsoft.Extensions.Caching.Memory;
using RestSharp;
using System.Threading.Tasks;

namespace GOOM.TR.MyRetail.NET.Repos
{
    public interface IProductRepo
    {
        Task<Product> GetProductAsync(long id);
    }

    public class ProductRepo : IProductRepo
    {
        private static readonly string Excludes = string.Join(",", new string[]
        {
            "available_to_promise_network",
            "taxonomy",
            "price",
            "promotion",
            "bulk_ship",
            "rating_and_review_reviews",
            "rating_and_review_statistics",
            "question_answer_statistics",
            "available_to_promise_network"
        });

        private readonly IMemoryCache _cache;
        private readonly IRestClient _client;
        private readonly string _key = "candidate";

        public ProductRepo(IMemoryCache cache)
        {
            _cache = cache;
            _client = new RestClient("https://redsky.target.com/");
        }

        public async Task<Product> GetProductAsync(long id)
        {
            var name = await GetProductNameAsync(id);
            return name != null
                ? new Product { Id = id, Name = name}
                : null;
        }

        private async Task<string> GetProductNameAsync(long id)
        {
            var key = $"products/{id}/name";
            var name = _cache.Get<string>(key);

            if (name == null)
            {
                var request = new RestRequest($"v3/pdp/tcin/{id}?excludes={Excludes}&key={_key}#_blank");
                var productResponse = await _client.GetAsync<ProductResponse>(request);
                if (productResponse.Product.Item.ErrorMessage == null)
                {
                    name = productResponse.Product.Item.ProductDescription.Title;
                    _cache.Set(key, name);
                }
            }

            return name;
        }

        private class ProductResponse
        {
            public RedskyProduct Product { get; set; }
        }

        private class RedskyProduct
        {
            public Item Item { get; set; }
        }

        private class Item
        {
            public ProductDescription ProductDescription { get; set; }
            public string ErrorMessage { get; set; }
            public int StatusCode { get; set; }
        }

        private class ProductDescription 
        {
            public string Title { get; set; }
        }
    }
}
