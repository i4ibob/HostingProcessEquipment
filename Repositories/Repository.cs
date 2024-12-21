using JuniorDotNetTestTaskServiceHostingProcessEquipment.Data;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Models;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JuniorDotNetTestTaskServiceHostingProcessEquipment.Repositories
{

    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbSet<T> _dbSet; 


        public Repository(DbContext context)
        {
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> Query() => _dbSet.AsQueryable();

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching entity: {ex.Message}");
                throw;
            }
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Remove(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Update(entity);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }


        public async Task<decimal> CalculateAvailableAreaAsync(int productionFacilityId, DbContext context)
        {
            var facility = await context.Set<ProductionFacility>()
                .Include(f => f.Contracts)
                .ThenInclude(c => c.ProcessEquipment)
                .FirstOrDefaultAsync(f => f.Id == productionFacilityId);

            if (facility == null)
                throw new ArgumentException("Production facility not found.");

            var occupiedArea = facility.Contracts.Sum(c => c.Quantity * c.ProcessEquipment.Area);

            return facility.StandardArea - occupiedArea;
        }

    }
      



}
