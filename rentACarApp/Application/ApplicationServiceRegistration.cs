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
using Application.Services.AuthService;
using Freezone.Core.Application.Pipelines.Authorization;
using Freezone.Core.Application.Pipelines.Transaction;
using Freezone.Core.Application.Pipelines.Caching;
using Freezone.Core.Mailing;
using Freezone.Core.Mailing.MailKit;
using Freezone.Core.Security.Authenticator.Email;
using Freezone.Core.Security.Authenticator.Otp;
using Freezone.Core.Security.JWT;

namespace Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>)
                ,typeof(RequestValidationBehavior<,>));

            services.AddTransient(typeof(IPipelineBehavior<,>)
                , typeof(LoggingBehavior<,>));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionScopeBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheRemovingBehavior<,>));

            services.AddSubClassesOfType(Assembly.GetExecutingAssembly()
                , typeof(BaseBusinessRules));

            services.AddSingleton<LoggerServiceBase, FileLogger>();

            services.AddScoped<ITokenHelper, JwtHelper>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailAuthenticatorHelper, EmailAuthenticatorHelper>();
            services.AddScoped<IMailService, MailKitMailService>();
            services.AddScoped<IOtpAuthenticatorHelper, OtpAuthenticatorHelper>();

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
