using GOOM.TR.MyRetail.NET.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.IdGenerators;

namespace GOOM.TR.MyRetail.NET.Repos
{
    public interface IPriceRepo
    {
        Task<Price> GetPriceByProductIdAsync(long productId);
        Task<Price> SetProductPriceAsync(long productId, Price price);
        Task<bool> DeleteProductPriceAsync(long productId);
    }

    public class PriceRepo : IPriceRepo
    {
        private readonly IMongoCollection<DbPrice> prices;

        public PriceRepo(IPriceStoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            prices = database.GetCollection<DbPrice>(settings.PricesCollectionName);
        }
        
        public async Task<Price> GetPriceByProductIdAsync(long productId)
        {
            var dbPrice = (await prices.FindAsync(x => x.ProductId == productId)).SingleOrDefault();
            return dbPrice == null
                ? null
                : new Price
                {
                    CurrencyCode = dbPrice.CurrencyCode,
                    Value =  dbPrice.Value
                };
        }

        public async Task<Price> SetProductPriceAsync(long productId, Price price)
        {
            var result = await prices.ReplaceOneAsync(x => x.ProductId == productId,
                new DbPrice()
                {
                    ProductId = productId,
                    CurrencyCode = price.CurrencyCode, 
                    Value = price.Value
                }, new ReplaceOptions() { IsUpsert = true});
            
            return result.IsAcknowledged ? price : null;
        }

        public async Task<bool> DeleteProductPriceAsync(long productId)
        {
            var result = await prices.DeleteOneAsync(x => x.ProductId == productId);

            return result.DeletedCount == 1;
        }
    }

    public class DbPrice
    {
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }

        public long ProductId { get; set; }

        public decimal Value { get; set; }

        public string CurrencyCode { get; set; }
    }

    public class PriceStoreDatabaseSettings : IPriceStoreDatabaseSettings
    {
        public string PricesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IPriceStoreDatabaseSettings
    {
        string PricesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
