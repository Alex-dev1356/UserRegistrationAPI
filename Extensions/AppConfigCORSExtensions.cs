namespace AuthECAPI.Extensions
{
    public static class AppConfigCORSExtensions
    {
        public static WebApplication CORSConfig(this WebApplication app, 
                                                IConfiguration config) //Adding extra parameter for IConfiguration to get the CORS settings from appsettings.json
        {
            app.UseCors(options =>
                options.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader());
            return app;
        }
    }
}
