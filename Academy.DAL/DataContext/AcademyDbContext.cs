using Academy.DAL.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.DAL.DataContext
{
    public class AcademyDbContext : DbContext
    {
        public AcademyDbContext(DbContextOptions<AcademyDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
    }
}
