using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PROG7311_GLMSApp.Services;
using static PROG7311_GLMSApp.Services.ConcreteContract;
namespace PROG7311_GLMSApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            builder.Services.AddHttpContextAccessor();

            var apiUrl = builder.Configuration["ApiUrl"] ?? "https://localhost:7256/";

            builder.Services.AddHttpClient<CurrencyService>("ExchangeRateApi", client =>
            {
                client.BaseAddress = new Uri("https://v6.exchangerate-api.com/v6/");
            });

            builder.Services.AddHttpClient<ContractService>(client =>
            {
                client.BaseAddress = new Uri(apiUrl);                
            });

            builder.Services.AddHttpClient<ClientService>(client =>
            {
                client.BaseAddress = new Uri(apiUrl);
            });

            builder.Services.AddHttpClient<ServiceRequestService>(client =>
            {
                client.BaseAddress = new Uri(apiUrl);
            });


            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IContractFactory, ContractFactory>();  
            builder.Services.AddScoped<ContractContext>();
            builder.Services.AddScoped<Notifier>();
            



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

          
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
           
            app.Run();
        }
    }
}
