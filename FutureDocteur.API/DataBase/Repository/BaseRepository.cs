using FutureDocteur.API.DataBase.Repository.Contract;
using FutureDocteur.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FutureDocteur.API.DataBase.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly FutureDoctorDbContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(FutureDoctorDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
        public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);
        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
        public void Update(T entity) => _dbSet.Update(entity);
        public void Delete(T entity) => _dbSet.Remove(entity);
        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }

}
