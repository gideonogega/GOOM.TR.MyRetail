using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using GOOM.TR.MyRetail.NET.Models;
using GOOM.TR.MyRetail.NET.Repos;
using GOOM.TR.MyRetail.NET.Tests.Helpers;
using MongoDB.Driver;
using NUnit.Framework;

namespace GOOM.TR.MyRetail.NET.Tests.Integration
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class ProductsApiTests
    {
        private ApiFixture _apiFixture;
        private TestClient _client;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _apiFixture = new ApiFixture();
        }

        [SetUp]
        public void Setup()
        {
            _client = _apiFixture.GetClient();
        }

        [Test]
        public async Task Get_InvalidProductId_ReturnsNull()
        {
            // Act
            var result = await _client.GetAsync<Product>($"products/{long.MinValue}");

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task Get_ProductWithoutPrice_ReturnsPartialResponse()
        {
            // Arrange 
            var expected = new Product
            {
                Id = 13860428,
                Name = "The Big Lebowski (Blu-ray)"
            };

            // Act
            var result = await _client.GetAsync<Product>($"products/{expected.Id}");

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task Get_ProductWithPrice_ReturnsFullResponse()
        {
            // Arrange 
            var expected = new Product
            {
                Id = 54456119,
                Name = "Creamy Peanut Butter 40oz - Good &#38; Gather&#8482;",
                CurrentPrice =  new Price
                {
                    CurrencyCode = "USD",
                    Value = 6.99m
                }
            };
            await _apiFixture.Prices.ReplaceOneAsync(x => x.ProductId == expected.Id,
                new DbPrice()
                {
                    ProductId = expected.Id,
                    CurrencyCode = expected.CurrentPrice.CurrencyCode,
                    Value = expected.CurrentPrice.Value
                }, new ReplaceOptions() { IsUpsert = true });

            // Act
            var result = await _client.GetAsync<Product>($"products/{expected.Id}");

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            //_apiFixture.Dispose();
        }
    }
}