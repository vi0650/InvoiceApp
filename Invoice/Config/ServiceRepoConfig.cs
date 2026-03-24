using Invoice.Repository;
using Invoice.Repository.IRepository;
using Invoice.Services;
using Invoice.Services.IServices;
using Microsoft.Extensions.DependencyInjection;

namespace Invoice.Config
{
    public static class ServiceRepoConfig
    {
        public static IServiceCollection ServiceRepoConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICustomerRepo, CustomerRepo>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IInvoiceRepo, InoviceRepo>();
            services.AddScoped<IGstRepo,GstRepo>();
            services.AddScoped<IGstService,GstService>();
            //services.AddScoped<IInvoiceItemService, InvoiceItemService>();
            return services;
        }
    }
}
