using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Common.Settings;

namespace Play.Common.Mongo
{
  public static class Extensions {
    public static IServiceCollection AddMongo(this IServiceCollection services) {
      BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.BsonType.String));
      BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(MongoDB.Bson.BsonType.String));

      services.AddSingleton(serviceProvider => {
        var configuration = serviceProvider.GetService<IConfiguration>();
        var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
        var mongodbSettings = configuration.GetSection(nameof(MongodbSettings)).Get<MongodbSettings>();
        var mongoClient = new MongoClient(mongodbSettings.connectionStr);
        return mongoClient.GetDatabase(serviceSettings.serviceName);
      });

      return services;
    }

    public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) where T : IEntity {
      services.AddSingleton<IRepository<T>>(sp => {
        var db = sp.GetService<IMongoDatabase>();
        return new MongoRepository<T>(db, "items");
      });
      
      return services;
    }
  }
  
}