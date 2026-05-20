
using AuthECAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace AuthECAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Adding Services from Identity Core
            //For Identity Manager
            builder.Services
                .AddIdentityApiEndpoints<AppUser>()//This is responsible for adding the endpoints for Identity API Core
                                                        //The IdentityUser is the default user class provided by ASP.NET Core Identity.
                                                        //It includes properties like UserName, Email, PasswordHash, etc.
                                                        //You can also create a custom user class that inherits from IdentityUser
                                                        //if you need to add additional properties specific to your application.
            .AddEntityFrameworkStores<ApplicationDbContext>(); //This is responsible for adding the stores for Identity API Core
                                                               //The ApplicationDbContext is the database context that will be used to store the identity data.
                                                               //You need to create this class and configure it to use your database provider (e.g., SQL Server, SQLite, etc.)
                                                               //It should inherit from IdentityDbContext<IdentityUser> or a custom user class if you created one.

            //This is responsible for configuring different options for Identity API Core
            builder.Services.Configure<IdentityOptions>(options => 
                {
                    options.Password.RequireDigit = false; //This is responsible for making sure that the password does not require a digit in Identity API Core
                    options.Password.RequireLowercase = false; //This is responsible for making sure that the password does not require a lowercase letter in Identity API Core
                    options.Password.RequireUppercase = false; //This is responsible for making sure that the password does not require an uppercase letter in Identity API Core
                    options.Password.RequireNonAlphanumeric = false; //This is responsible for making sure that the password does not require a non-alphanumeric character in Identity API Core
                    options.User.RequireUniqueEmail = true; //This is responsible for making sure that each user has a unique email address in Identity API Core
                });

            //Adding the database context to the services container
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DevDB")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //Adding the CORS policy to allow requests from the frontend application
            #region Config. CORS
            app.UseCors(options => 
                options.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader());
            #endregion

            app.UseAuthorization();

            app.MapControllers();

            //Adding the respective Routes for Identity API Core
            app
                .MapGroup("/api") //This is responsible for grouping the endpoints for Identity API Core under the "/api" route
                .MapIdentityApi<AppUser>(); //This is responsible for mapping the endpoints for Identity API Core
                                            //The IdentityUser is the default user class provided by ASP.NET Core Identity.
                                            //It includes properties like UserName, Email, PasswordHash, etc.
                                            //You can also create a custom user class that inherits from IdentityUser
                                            //if you need to add additional properties specific to your application.

            //Using Minimal API Controllers for Identity API Core
            app.MapPost("/api/signup", async(
                UserManager<AppUser> userManager, //This is responsible for managing the users in Identity API Core
                [FromBody] UserRegistrationModel userRegistrationModel //This is responsible for binding the data from the request body to the UserRegistrationModel class
                ) => 
            {
                //Creating a new user object and populating it with the data from the request body
                AppUser user = new AppUser()
                {
                    UserName = userRegistrationModel.Email, //Setting the UserName property to the email address of the user
                    Email = userRegistrationModel.Email,
                    FullName = userRegistrationModel.FullName
                };
                //The UserManager class provides various methods for managing users, such as creating, updating, deleting, etc.
                var result = await userManager.CreateAsync(
                    user, 
                    userRegistrationModel.Password); //This is responsible for creating a new user in Identity API Core
                                                     //The CreateAsync method takes the user object and the password as parameters and creates a new user in the database.
                                                     //It returns a Task<IdentityResult> which indicates whether the operation was successful or not.
                                                     //You can also handle the result of the operation to return appropriate responses to the client.

                //returning the result of the operation to the client
                if (result.Succeeded)
                {
                    return Results.Ok(result);
                }
                else
                {
                    return Results.BadRequest(result.Errors);
                }
            });


            app.Run();
        
        }
    }
}
