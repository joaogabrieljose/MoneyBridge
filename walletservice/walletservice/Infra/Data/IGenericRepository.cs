namespace walletservice.Infra.Data;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(long id);

    Task<List<T>> GetAllAsync();

    Task AddAsync(T entity);

    void Update(T entity);

    void Delete(T entity);

    Task SaveAsync();
    
}