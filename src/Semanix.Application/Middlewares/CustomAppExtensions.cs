using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Semanix.Application.Middlewares
{
    public class CustomAppExtensions
    {
        //public static IServiceCollection AddCustomMediatR(this IServiceCollection services, params Assembly[] assemblies)
        //{
        //    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

        //    services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        //    services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        //    services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

        //    return services;
        //}
    }
}
