using Academy.DAL.DataContext;
using Academy.DAL.DataContext.Entities;
using Academy.DAL.Repositories.Interfaces;
using Core.Persistence.Repositories;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Academy.DAL.Repositories.Implementations
{
    public class AttendanceRepository : EfCoreRepositoryAsync<Attendance, AcademyDbContext>, IAttendanceRepository
    {
        private readonly AcademyDbContext _db;

        public AttendanceRepository(AcademyDbContext context) : base(context)
        {
            _db = context;
        }

        public override async Task<IEnumerable<Attendance>> GetAllAsync(params Expression<Func<Attendance, object>>[] includes)
        {
            // API 500 error-ların qarşısını almaq üçün birbaşa DbContext vasitəsilə təhlükəsiz (.ThenInclude) çəkirik:
            return await _db.Attendances
                .AsNoTracking()
                .Include(a => a.Student)
                    .ThenInclude(s => s.Group)
                .ToListAsync();
        }
    }
}
