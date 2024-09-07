using Microsoft.EntityFrameworkCore;
using Todo.Api.Crud.Todo;
using Todo.Api.Data;
using Todo.Api.Search.Todo;

namespace Todo.Api.AppStart
{
    public static class DependencyConfiguration
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<TodoCrud>();
            services.AddScoped<TodoSearch>();

            // Register the full CSV file path as a singleton
            services.AddSingleton(provider =>
            {
                var csvFilePath = configuration.GetValue<string>("CsvFilePath") ?? string.Empty;
                var basePath = Directory.GetCurrentDirectory();
                return Path.Combine(basePath, csvFilePath);
            });

            // Configure DbContextOptions<TodoContext>
            services.AddSingleton(provider =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<TodoContext>();
                optionsBuilder.UseInMemoryDatabase("TodoDatabase"); // Use an in-memory database for EF Core
                return optionsBuilder.Options;
            });

            // Register TodoContext with InMemoryDatabase
            services.AddDbContext<TodoContext>(options =>
                options.UseInMemoryDatabase("TodoDatabase"));                   
        }
    }
}