using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class Repository<T>(AppContext context) : IRepository<T> where T : BaseEntity
{
    // TODO: Save Changes and Dispose methods

    public void Add(T entity)
    {
        context.Set<T>().Add(entity);
    }

    public async Task<bool> Complete()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void Delete(T entity)
    {
        context.Set<T>().Remove(entity);
    }

    public void Dispose()
    {
        context.Dispose();
    }

    public void Edit(T entity)
    {
        context.Set<T>().Update(entity);
    }

    public bool Exists(int id)
    {
        return context.Set<T>().Any(x => x.Id == id);
    }

    public async Task<IList<T>> GetAllAsync()
    {
        return await context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await context.Set<T>().FindAsync(id);
    }
}
