using Academy.DAL.DataContext;
using Academy.DAL.DataContext.Entities;
using Academy.DAL.Repositories.Interfaces;
using Core.Persistence.Repositories;
using System.Linq.Expressions;

namespace Academy.DAL.Repositories.Implementations
{
    public class GroupRepository : EfCoreRepositoryAsync<Group, AcademyDbContext>, IGroupRepository
    {
        public GroupRepository(AcademyDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Group>> GetAllAsync(params Expression<Func<Group, object>>[] includes)
        {
            return await base.GetAllAsync(g => g.Students, g => g.Teacher);
        }
    }
}
