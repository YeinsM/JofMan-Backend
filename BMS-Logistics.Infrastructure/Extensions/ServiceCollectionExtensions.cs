using BMS_Logistics.API.Interfaces;
using BMS_Logistics.Application.Common.Authentication;
using BMS_Logistics.Application.Interfaces;
using BMS_Logistics.Application.Services;
using BMS_Logistics.Domain.Entities;
using BMS_Logistics.Infrastructure.Helpers;
using BMS_Logistics.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BMS_Logistics.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IGeneric<Status>, StatusRepo>();

            services.AddScoped<IGeneric<User>, UserRepo>();
            services.AddScoped<IUser, UserRepo>();

            services.AddScoped<IDecryptHelper, DecryptHelper>();

            services.AddScoped<ICurrentUserContext, CurrentUserContext>();
            services.AddScoped<GenericJwtConfig>();

            // servicio auditable
            services.AddScoped<IAuditableEntityService, AuditableEntityService>();

            return services;
        }
    }
}
