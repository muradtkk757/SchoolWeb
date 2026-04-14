using Academy.DAL.DataContext;
using Academy.DAL.DataContext.Entities;
using Academy.DAL.Repositories.Interfaces;
using Core.Persistence.Repositories;

namespace Academy.DAL.Repositories.Implementations
{
    public class GroupRepository : EfCoreRepositoryAsync<Group, AcademyDbContext>, IGroupRepository
    {
        public GroupRepository(AcademyDbContext context) : base(context)
        {
        }
    }
}
