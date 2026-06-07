
using API_Techmove.Data;
using Microsoft.EntityFrameworkCore;


namespace API_Techmove
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DataContext>();

                var retries = 5;
                while (retries > 0)
                {
                  try
                    {
                     db.Database.Migrate();
                     break;
                    }
                  catch
                    {
                     retries--;
                     Thread.Sleep(5000); 
                    }
                }
            }

            // Configure the HTTP request pipeline.    
            app.UseSwagger();
            app.UseSwaggerUI();            


            app.UseAuthorization();


            app.MapControllers();

            
            app.Run();
        }
    }
}
