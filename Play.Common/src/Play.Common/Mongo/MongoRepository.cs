using System.Linq.Expressions;
using MongoDB.Driver;

namespace Play.Common.Mongo
{

  public class MongoRepository<T> : IRepository<T> where T : IEntity
  {
    private readonly IMongoCollection<T> dbCollection;
    private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

    public MongoRepository(IMongoDatabase db, string collectionName)
    {
      dbCollection = db.GetCollection<T>(collectionName);
    }

    public async Task<IReadOnlyCollection<T>> getAllAsync()
    {
      return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
    }

    public async Task<IReadOnlyCollection<T>> getAllAsync(Expression<Func<T, bool>> filter)
    {
      return await dbCollection.Find(filter).ToListAsync();
    }

    public async Task<T> getAsync(Guid id)
    {
      FilterDefinition<T> filter = filterBuilder.Eq(entity => entity.id, id);
      return await dbCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<T> getAsync(Expression<Func<T, bool>> filter)
    {
      return await dbCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task createAsync(T entity)
    {
      if (entity == null) throw new ArgumentNullException(nameof(entity));
      await dbCollection.InsertOneAsync(entity);
    }

    public async Task updateAsync(T entity)
    {
      if (entity == null) throw new ArgumentNullException(nameof(entity));
      FilterDefinition<T> filter = filterBuilder.Eq(item => item.id, entity.id);
      await dbCollection.ReplaceOneAsync(!filter, entity);
    }

    public async Task deleteAsync(Guid id)
    {
      FilterDefinition<T> filter = filterBuilder.Eq(item => item.id, id);
      await dbCollection.DeleteOneAsync(filter);
    }
  }
}