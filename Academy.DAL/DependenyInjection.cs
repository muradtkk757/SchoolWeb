using Academy.DAL.DataContext;
using Academy.DAL.Repositories.Implementations;
using Academy.DAL.Repositories.Interfaces;
using Core.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.DAL
{
    public static class DependenyInjection
    {
        public static void AddDAL(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AcademyDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IRepositoryAsync<,>), typeof(EfCoreRepositoryAsync<,>));
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
        }
    }
}
