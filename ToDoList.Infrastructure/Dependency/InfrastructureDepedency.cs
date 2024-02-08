using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Application.Interfaces;
using ToDoList.Infrastructure.Persistence;
using ToDoList.Infrastructure.Services;

namespace ToDoList.Infrastructure.Dependency
{
    public static class InfrastructureDepedency
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            var connString = config.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connString, b => b.MigrationsAssembly("ToDoList.Infrastructure"))
            );

            services.AddTransient<IToDoService, ToDoService>();

            services.AddTransient<IUserService, UserService>();


            services.AddTransient<IGenericRepository, GenericRepository>();

            return services;
        }
    }
}
