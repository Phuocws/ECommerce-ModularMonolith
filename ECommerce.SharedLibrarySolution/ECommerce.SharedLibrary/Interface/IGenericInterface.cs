using System.Linq.Expressions;

namespace ECommerce.SharedLibrary.Interface
{
	public interface IGenericInterface<T> where T : class
	{
		Task<Response.Response> CreateAsync(T entity);
		Task<Response.Response> UpdateAsync(T entity);
		Task<Response.Response> DeleteAsync(int id);
		Task<T> GetByIdAsync(int id);
		Task<IEnumerable<T>> GetAllAsync();
		Task<T> GetByConditionAsync(Expression<Func<T, bool>> predicate);
	}
}
