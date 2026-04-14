using Academy.DAL.DataContext;
using Academy.DAL.DataContext.Entities;
using Academy.DAL.Repositories.Interfaces;
using Core.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.DAL.Repositories.Implementations
{
    public class StudentRepository : EfCoreRepositoryAsync<Student, AcademyDbContext>, IStudentRepository
    {
        public StudentRepository(AcademyDbContext context) : base(context)
        {
        }
    }
}
