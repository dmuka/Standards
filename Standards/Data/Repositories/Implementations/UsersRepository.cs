using Microsoft.EntityFrameworkCore;
using Standards.Data.Repositories.Interfaces;
using Standards.Extensions;
using Standards.Models.Users;
using System.Linq.Expressions;

namespace Standards.Data.Repositories.Implementations
{
    public class UsersRepository : IRepository<User>, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private bool _disposed = false;

        public UsersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAll() => _context.Users.ToList();

        public Task<List<User>> GetAllAsync() => _context.Users.ToListAsync();

        public User? GetById(int id) => _context.Users.Find(id);

        public User? GetByIdWithIncludes(int id)
        {
            return _context.Users.Include(user => user.Role)
                .FirstOrDefault(user => user.Id == id);
        }

        public async Task<User> GetByIdAsync(int id) => await _context.Users.FindAsync(id);

        public async Task<User> GetByIdWithIncludesAsync(int id)
        {
            return await _context.Users.Include(user => user.Role)
                .FirstOrDefaultAsync(user => user.Id == id);
        }

        public bool Remove(int id)
        {
            var user = _context.Users.Find(id);
            if (user is { })
            {
                _context.Users.Remove(user);
                return true;
            }

            return false;
        }

        public void Add(in User user)
        {
            _context.Add(user).State = EntityState.Added;
        }

        public void Update(in User user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        public User Select(Expression<Func<User, bool>> predicate)
        {
            return _context.Users.WhereNullSafe(predicate).FirstOrDefault()!;
        }

        public async Task<User> SelectAsync(Expression<Func<User, bool>> predicate)
        {
            return
                (
                    await _context.Users.WhereNullSafe(predicate).FirstOrDefaultAsync())!;
        }

        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing) _context.Dispose();

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
