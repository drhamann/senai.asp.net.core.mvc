namespace Authentication.Domain.Repositories
{
    public interface IBaseRepository<T>
    {
        Task<T> GetAsync();
        Task<T> GetByIdAsync(Guid id);
        Task Create(T entity);
        Task<T> Update(T entity);
        Task<bool> Delete(Guid id);

    }
}
