using Academy.BLL.Mapping;
using Academy.BLL.Services.Implementations;
using Academy.BLL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.BLL
{
    public static class DependencyInjection
    {
        public static void AddBLL(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            { 
                cfg.AddMaps(typeof(MappingProfile).Assembly);
            }); 
            services.AddScoped(typeof(ICrudServiceAsync<,,,>), typeof(CrudManager<,,,>));
            services.AddScoped<IGroupService, GroupManager>();
        }
    }
}
