using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using GOOM.TR.MyRetail.NET.Models;
using GOOM.TR.MyRetail.NET.Repos;
using GOOM.TR.MyRetail.NET.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace GOOM.TR.MyRetail.NET.Tests.Unit
{
    public class Tests
    {
        private static Fixture Fixture = new Fixture();

        private IProductService _service;
        private Mock<ILogger<ProductService>> _log;
        private Mock<IProductRepo> _productRepo;
        private Mock<IPriceRepo> _priceRepo;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _log = new Mock<ILogger<ProductService>>();
            _productRepo = new Mock<IProductRepo>();
            _priceRepo = new Mock<IPriceRepo>();
            _service = new ProductService(_log.Object, _productRepo.Object, _priceRepo.Object);
        }

        [Test]
        public async Task GetProductAsync_WhenNull_ReturnsNull()
        {
            // Arrange
            var productId = Fixture.Create<long>();

            // Act
            var result = await _service.GetProductAsync(productId);

            // Assert
            result.Should().BeNull();
            _productRepo.Verify(x => x.GetProductAsync(productId), Times.Once);
            _priceRepo.Verify(x => x.GetPriceByProductIdAsync(productId), Times.Never);
        }

        [Test]
        public async Task GetProductAsync_WhenProductWithNoPrice_ReturnsPartial()
        {
            // Arrange
            var product = new Product
            {
                Id = Fixture.Create<long>(),
                Name = Fixture.Create<string>()
            };
            _productRepo.Setup(x => x.GetProductAsync(product.Id))
                .Returns(Task.FromResult(product));

            // Act
            var result = await _service.GetProductAsync(product.Id);

            // Assert
            result.Should().BeEquivalentTo(new Product
            {
                Id = product.Id,
                Name = product.Name
            });
            _productRepo.Verify(x => x.GetProductAsync(product.Id), Times.Once);
            _priceRepo.Verify(x => x.GetPriceByProductIdAsync(product.Id), Times.Once);
        }

        [Test]
        public async Task GetProductAsync_WhenProductWithPrice_ReturnsFull()
        {
            // Arrange
            var product = new Product
            {
                Id = Fixture.Create<long>(),
                Name = Fixture.Create<string>()
            };
            var price = new Price
            {
                Value = Fixture.Create<decimal>(),
                CurrencyCode = Fixture.Create<string>()
            };
            _productRepo.Setup(x => x.GetProductAsync(product.Id))
                .Returns(Task.FromResult(product));
            _priceRepo.Setup(x => x.GetPriceByProductIdAsync(product.Id))
                .Returns(Task.FromResult(price));

            // Act
            var result = await _service.GetProductAsync(product.Id);

            // Assert
            result.Should().BeEquivalentTo(new Product
            {
                Id = product.Id,
                Name = product.Name,
                CurrentPrice = new Price
                {
                    CurrencyCode = price.CurrencyCode,
                    Value = price.Value
                }
            });
            _productRepo.Verify(x => x.GetProductAsync(product.Id), Times.Once);
            _priceRepo.Verify(x => x.GetPriceByProductIdAsync(product.Id), Times.Once);
        }
    }
}