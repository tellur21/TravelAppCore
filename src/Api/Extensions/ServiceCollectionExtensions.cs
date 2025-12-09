using Application.Interfaces;
using FluentValidation;
using Infrastructure.Services;
using Persistence.Repositories;
using System.Reflection;

namespace Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("Application")));

            // FluentValidation
            services.AddValidatorsFromAssembly(Assembly.Load("Application"));

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IPaymentService, StripePaymentService>();

            return services;
        }

        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            services.AddScoped<ITravelPackageRepository, TravelPackageRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();

            return services;
        }
    }
}
