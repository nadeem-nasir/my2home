using Microsoft.Extensions.DependencyInjection;
using My2Home.Core.Interfaces;
using My2Home.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace My2Home.Infrastructure.Extensions
{
    public static class AddRepositoryExtension
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {           
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();


            services.AddScoped<IBedRepository, BedRepository>();

            services.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();
            services.AddScoped<IExpenseCategoryRepository, ExpenseCategoryRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IHostelRepository, HostelRepository>();
            services.AddScoped<IHostelsDocumentRepository, HostelsDocumentRepository>();
            services.AddScoped<IMessSubscriptionRepository, MessSubscriptionRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IOrganizationUserRepository, OrganizationUserRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();

            services.AddScoped<ITenantRepository, TenantRepository>();

            services.AddScoped<IRentRepository, RentRepository>();

            services.AddScoped<IDashboardRepository, DashboardRepository>();

            return services;
        }
    }
}
