using Academy.DAL.DataContext;
using Academy.DAL.DataContext.Entities;
using Core.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.DAL.Repositories.Interfaces
{
    public interface IStudentRepository : IRepositoryAsync<Student, AcademyDbContext>
    {
    }
}
