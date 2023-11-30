using ErrorOr;

namespace DropShipping.DAOs;

public interface DAO<T>
{
	Task<ErrorOr<T>> GetById(long id);

	Task<ErrorOr<List<T>>> GetAll(int pageNumber);

	Task<ErrorOr<bool>> Save(T t);

	Task<ErrorOr<T>> Upsert(T t);

	Task<ErrorOr<bool>> Delete(long id);

}