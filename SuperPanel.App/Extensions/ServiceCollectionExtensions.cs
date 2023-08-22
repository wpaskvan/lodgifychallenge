using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SuperPanel.App.Mapping;

namespace SuperPanel.App.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureAutomapper(this IServiceCollection services)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserMappingProfile>();
            });

            var mapper = new Mapper(configuration);

            services.AddSingleton<IMapper>(mapper);
        }
    }
}
