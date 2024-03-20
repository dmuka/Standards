using Microsoft.EntityFrameworkCore;
using Standards.Data.Repositories.Interfaces;
using Standards.Extensions;
using Standards.Models.Persons;
using System.Linq.Expressions;

namespace Standards.Data.Repositories.Implementations
{
    public class UsersRepository : IRepository<Person>, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private bool _disposed = false;

        public UsersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Person> GetAll() => _context.Persons.ToList();

        public Task<List<Person>> GetAllAsync() => _context.Persons.ToListAsync();

        public Person GetById(int id) => _context.Persons.Find(id);

        public Person GetByIdWithIncludes(int id)
        {
            return _context.Persons.Include(person => person.RoleId)
                .FirstOrDefault(person => int.Parse(person.Id) == id);
        }

        public async Task<Person> GetByIdAsync(int id) => await _context.Persons.FindAsync(id);

        public async Task<Person> GetByIdWithIncludesAsync(int id)
        {
            return await _context.Persons.Include(person => person.RoleId)
                .FirstOrDefaultAsync(person => int.Parse(person.Id) == id);
        }

        public bool Remove(int id)
        {
            var Person = _context.Persons.Find(id);
            if (Person is { })
            {
                _context.Persons.Remove(Person);
                return true;
            }

            return false;
        }

        public void Add(in Person sender)
        {
            _context.Add(sender).State = EntityState.Added;
        }

        public void Update(in Person sender)
        {
            _context.Entry(sender).State = EntityState.Modified;
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Person Select(Expression<Func<Person, bool>> predicate)
        {
            return _context.Persons.WhereNullSafe(predicate).FirstOrDefault()!;
        }

        public async Task<Person> SelectAsync(Expression<Func<Person, bool>> predicate)
        {
            return
                (
                    await _context.Persons.WhereNullSafe(predicate).FirstOrDefaultAsync())!;
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
