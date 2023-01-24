using FluentValidation;
using Freezone.Core.Application.Pipelines.Logging;
using Freezone.Core.Application.Pipelines.Validation;
using Freezone.Core.Application.Rules;
using Freezone.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Freezone.Core.CrossCuttingConcerns.Logging.Serilog;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Freezone.Core.Application.Pipelines.Transaction;

namespace Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicatioonServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>)
                ,typeof(RequestValidationBehavior<,>));

            services.AddTransient(typeof(IPipelineBehavior<,>)
                , typeof(LoggingBehavior<,>));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionScopeBehavior<,>));

            services.AddSubClassesOfType(Assembly.GetExecutingAssembly()
                , typeof(BaseBusinessRules));

            services.AddSingleton<LoggerServiceBase, FileLogger>();


            return services;

        }

        public static IServiceCollection AddSubClassesOfType(
       this IServiceCollection services,
       Assembly assembly,
       Type type,
       Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null)
        {
            var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
            foreach (var item in types)
            {
                if (addWithLifeCycle == null)
                {
                    services.AddScoped(item);
                }
                else
                {
                    addWithLifeCycle(services, type);
                }
            }
            return services;
        }
    }
}
