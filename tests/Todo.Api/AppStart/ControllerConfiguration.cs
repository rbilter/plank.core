namespace Todo.Api.AppStart
{
    public class ControllerConfiguration
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddControllers();
        }
    }
}