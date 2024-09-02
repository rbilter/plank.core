namespace Todo.Api.AppStart
{
    public class SwaggerConfiguration
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddSwaggerGen();
        }
    }
}