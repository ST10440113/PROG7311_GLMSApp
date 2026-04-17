using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PROG7311_GLMSApp.Data;
using PROG7311_GLMSApp.Services;
using System.Diagnostics.Contracts;
namespace PROG7311_GLMSApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<PROG7311_GLMSAppContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("PROG7311_GLMSAppContext") ?? throw new InvalidOperationException("Connection string 'PROG7311_GLMSAppContext' not found.")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ContractService>();
            builder.Services.AddScoped<ClientService>();
            builder.Services.AddScoped<ServiceRequestService>();
            builder.Services.AddScoped<IContractFactory, ContractFactory>();
            builder.Services.AddScoped<ContractContext>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
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
