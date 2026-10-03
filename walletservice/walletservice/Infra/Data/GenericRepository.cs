using Microsoft.EntityFrameworkCore;

namespace walletservice.Infra.Data;

public abstract class GenericRepository<T> : IDisposable, IGenericRepository<T> where T : class
{ 
    private readonly ConnectionContext _context;

    protected GenericRepository(ConnectionContext context)
    {
        _context = context;
    }
    
    public void Dispose()
    {
        throw new NotImplementedException();
    }
    

    public async Task<T?> GetByIdAsync(long id)
    {
       return await _context.Set<T>().FindAsync(id);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _context.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}