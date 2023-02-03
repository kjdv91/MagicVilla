using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Village_API.Datos;
using Village_API.Repository.IRepository;

namespace Village_API.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        //inject DbContext
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public async Task Create(T entity)
        {
            await dbSet.AddAsync(entity);
            await Save();
        }

        public async Task<T> Get(Expression<Func<T, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();

            }
            if(filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
            
        }

        public async Task<List<T>> GetAlls(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task Remove(T entity)
        {

            dbSet.Remove(entity);
            await Save();

        }

        public async Task Save()
        {

            await _db.SaveChangesAsync();
        }
    }
}
