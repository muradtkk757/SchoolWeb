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
        public AttendanceRepository(AcademyDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Attendance>> GetAllAsync(params Expression<Func<Attendance, object>>[] includes)
        {
            // Tələbə və onun qrupunun məlumatlarını da birlikdə çəkirik ki Dto mapping-də Name-lər null gəlməsin
            return await base.GetAllAsync(a => a.Student, a => a.Student!.Group!);
        }
    }
}
