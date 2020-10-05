using GOOM.TR.MyRetail.NET.Repos;
using GOOM.TR.MyRetail.NET.Tests.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using MongoDB.Driver;

namespace GOOM.TR.MyRetail.NET.Tests.Integration
{
    public class ApiFixture : IDisposable
    {
        public readonly TestServer Server;
        public readonly IPriceStoreDatabaseSettings DatabaseSettings;
        public readonly IMongoCollection<DbPrice> Prices;

        public ApiFixture()
        {
            DatabaseSettings = new PriceStoreDatabaseSettings
            {
                PricesCollectionName = "Prices",
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "MyRetailDb_Test"
            };

            var client = new MongoClient(DatabaseSettings.ConnectionString);
            var database = client.GetDatabase(DatabaseSettings.DatabaseName);
            Prices = database.GetCollection<DbPrice>(DatabaseSettings.PricesCollectionName);

            var builder = new WebHostBuilder()
                .UseStartup<Startup>();
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(DatabaseSettings);
            });

            Server = new TestServer(builder);
        }

        public virtual TestClient GetClient()
        {
            return new TestClient(Server.CreateClient());
        }

        public void Dispose()
        {
            var client = new MongoClient(DatabaseSettings.ConnectionString);
            client.DropDatabase(DatabaseSettings.DatabaseName);
        }
    }
}
