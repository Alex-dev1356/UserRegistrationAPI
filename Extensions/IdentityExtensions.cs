using AuthECAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthECAPI.Extensions
{
    public static class IdentityExtensions
    {
        //Here we'll ONLY be having extension methods related to ASP.NET Core Identity, so we can keep this class clean and organized.

        //We'll be adding extenstion method for Identity Handlers and Corresponding Entity Framework Stores.
        public static IServiceCollection AddIdentityHandlersAndStores(this IServiceCollection services) //Here we use the 'this' keyword to indicate that this is an extension method for the IServiceCollection interface.
        {
            services.AddIdentityApiEndpoints<AppUser>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();
            return services;
        }


        //Adding Extension Method for configuring different options for Identity API Core
        public static IServiceCollection ConfigureIdentityOptions(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
                {
                    options.Password.RequireDigit = false; //This is responsible for making sure that the password does not require a digit in Identity API Core
                    options.Password.RequireLowercase = false; //This is responsible for making sure that the password does not require a lowercase letter in Identity API Core
                    options.Password.RequireUppercase = false; //This is responsible for making sure that the password does not require an uppercase letter in Identity API Core
                    options.Password.RequireNonAlphanumeric = false; //This is responsible for making sure that the password does not require a non-alphanumeric character in Identity API Core
                    options.User.RequireUniqueEmail = true; //This is responsible for making sure that each user has a unique email address in Identity API Core
                });
            return services;
        }


        //Adding Extension Method for configuring JWT Authentication Options for Identity API Core
        public static IServiceCollection AddIdentityAuth(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(x =>
            {
                //Passing Authentication Options to the AddAuthentication method
                x.DefaultAuthenticateScheme =
                x.DefaultChallengeScheme =
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; //This is responsible for setting the default authentication scheme to JWT Bearer in Identity API Core
            }).AddJwtBearer(y =>
            {
                y.SaveToken = false; //This is responsible for not saving the token in the HttpContext after a successful authentication in Identity API Core
                y.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, //This is responsible for validating the signing key of the token in Identity API Core
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes( //This is responsible for creating a new symmetric security key using the secret key defined in the appsettings.json file in Identity API Core
                            config["AppSettings:JWTSecret"]!) //This is responsible for getting the secret key from the appsettings.json file using the Configuration object in Identity API Core
                        )

                };

            });
            return services;
        }

        //Adding Extention Mehtod for the app.Authentication and app.Authorization middlewares in the Program.cs file for Identity API Core
        public static WebApplication AddIdentityAuthMiddlewares(this WebApplication app)
        {
            app.UseAuthentication();
            
            app.UseAuthorization();

            return app;
        }
    }
}
