using Microsoft.Extensions.DependencyInjection;
using SuperApp.Core.Implementations.Data;
using SuperApp.Core.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperApp.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureSuperAppCoreServices(this IServiceCollection services)
        {
            services.AddSingleton<IDatabase, SqliteDatabase>();
            services.AddSingleton<IUserRepository, UserRepository>();
        }
    }
}
