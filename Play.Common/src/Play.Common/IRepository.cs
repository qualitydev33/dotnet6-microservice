
using System.Linq.Expressions;

namespace Play.Common
{
  public interface IRepository<T> where T : IEntity
  {
    Task createAsync(T entity);
    Task deleteAsync(Guid id);
    Task<IReadOnlyCollection<T>> getAllAsync();
    Task<IReadOnlyCollection<T>> getAllAsync(Expression<Func<T, bool>> filter);
    Task<T> getAsync(Guid id);
    Task<T> getAsync(Expression<Func<T, bool>> filter);
    Task updateAsync(T entity);
  }
}