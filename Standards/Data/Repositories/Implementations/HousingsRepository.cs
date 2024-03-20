using Microsoft.EntityFrameworkCore;
using Standards.Data.Repositories.Interfaces;
using Standards.Extensions;
using Standards.Models.DTOs;
using System.Linq.Expressions;

namespace Standards.Data.Repositories.Implementations
{
    public class HousingsRepository : IRepository<HousingDto>, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private bool _disposed = false;

        public HousingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<HousingDto> GetAll() => _context.Housings.ToList();

        public Task<List<HousingDto>> GetAllAsync() => _context.Housings.ToListAsync();

        public HousingDto GetById(int id) => _context.Housings.Find(id);

        public HousingDto GetByIdWithIncludes(int id)
        {
            return _context.Housings.Include(Housing => Housing.Id)
                .FirstOrDefault(Housing => Housing.Id == id);
        }

        public async Task<HousingDto> GetByIdAsync(int id) => await _context.Housings.FindAsync(id);

        public async Task<HousingDto> GetByIdWithIncludesAsync(int id)
        {
            return await _context.Housings.Include(Housing => Housing.Id)
                .FirstOrDefaultAsync(Housing => Housing.Id == id);
        }

        public bool Remove(int id)
        {
            var Housing = _context.Housings.Find(id);
            if (Housing is { })
            {
                _context.Housings.Remove(Housing);
                return true;
            }

            return false;
        }

        public void Add(in HousingDto sender)
        {
            _context.Add(sender).State = EntityState.Added;
        }

        public void Update(in HousingDto sender)
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

        public HousingDto Select(Expression<Func<HousingDto, bool>> predicate)
        {
            return _context.Housings.WhereNullSafe(predicate).FirstOrDefault()!;
        }

        public async Task<HousingDto> SelectAsync(Expression<Func<HousingDto, bool>> predicate)
        {
            return
                (
                    await _context.Housings.WhereNullSafe(predicate).FirstOrDefaultAsync())!;
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