using System;
using Core.Entities;

namespace Core.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    Task<IList<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    void Add(T entity);
    void Edit(T entity);
    void Delete(T entity);
    bool Exists(int id);
    Task<bool> Complete();
    void Dispose();
}
