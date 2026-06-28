
using AuthECAPI.Controllers;
using AuthECAPI.Extensions;
using AuthECAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthECAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();


            //===================================================================================================
            //Adding Extension Method for Adding Swagger/OpenAPI Services
            #region AddSwaggerExplorer
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            #endregion

            //New Code Using Extension Method for Adding Swagger/OpenAPI Services
            builder.Services.AddSwaggerExplorer();
            //===================================================================================================


            //===================================================================================================
            //Adding the database context to the services container
            //<--Old Code-->
            #region AddDbContext
            //builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DevDB")));
            #endregion

            //New Code Using Extension Method for Adding Database Context
            builder.Services.InjectDBContext(builder.Configuration);
            //===================================================================================================


            //===================================================================================================
            //Adding Services from Identity Core
            //For Identity Manager
            //<--Old Code-->
            #region AddCascadingAuthenticationState
            //builder.Services
            //    .AddIdentityApiEndpoints<AppUser>()//This is responsible for adding the endpoints for Identity API Core
            //                                            //The IdentityUser is the default user class provided by ASP.NET Core Identity.
            //                                            //It includes properties like UserName, Email, PasswordHash, etc.
            //                                            //You can also create a custom user class that inherits from IdentityUser
            //                                            //if you need to add additional properties specific to your application.
            //.AddEntityFrameworkStores<ApplicationDbContext>(); //This is responsible for adding the stores for Identity API Core
            //                                                   //The ApplicationDbContext is the database context that will be used to store the identity data.
            //                                                   //You need to create this class and configure it to use your database provider (e.g., SQL Server, SQLite, etc.)
            //                                                   //It should inherit from IdentityDbContext<IdentityUser> or a custom user class if you created one.
            #endregion

            //New Code Using Extension Method for Identity Handlers and Corresponding Entity Framework Stores
            builder.Services.AddCascadingAuthenticationState();
            //===================================================================================================


            //===================================================================================================
            //This is responsible for configuring different options for Identity API Core
            //<--Old Code-->
            #region ConfigureIdentityOptions
            //builder.Services.Configure<IdentityOptions>(options => 
            //    {
            //        options.Password.RequireDigit = false; //This is responsible for making sure that the password does not require a digit in Identity API Core
            //        options.Password.RequireLowercase = false; //This is responsible for making sure that the password does not require a lowercase letter in Identity API Core
            //        options.Password.RequireUppercase = false; //This is responsible for making sure that the password does not require an uppercase letter in Identity API Core
            //        options.Password.RequireNonAlphanumeric = false; //This is responsible for making sure that the password does not require a non-alphanumeric character in Identity API Core
            //        options.User.RequireUniqueEmail = true; //This is responsible for making sure that each user has a unique email address in Identity API Core
            //    });
            #endregion 

            //New Code Using Extension Method for Configuring Identity Options
            builder.Services.ConfigureIdentityOptions();
            //===================================================================================================


            //===================================================================================================
            //With this, we will add the necessary services to the app and configured how we want to authenticate users in our application
            //and such logic will be executed by adding the UseAuthentication() in the middleware pipeline.
            //<--Old Code-->
            #region AddAuthentication
            //builder.Services.AddAuthentication(x => 
            //        {
            //            //Passing Authentication Options to the AddAuthentication method
            //            x.DefaultAuthenticateScheme =
            //            x.DefaultChallengeScheme =
            //            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; //This is responsible for setting the default authentication scheme to JWT Bearer in Identity API Core
            //        }).AddJwtBearer(y =>
            //        {
            //            y.SaveToken = false; //This is responsible for not saving the token in the HttpContext after a successful authentication in Identity API Core
            //            y.TokenValidationParameters = new TokenValidationParameters
            //            { 
            //                ValidateIssuerSigningKey = true, //This is responsible for validating the signing key of the token in Identity API Core
            //                IssuerSigningKey = new SymmetricSecurityKey(
            //                    Encoding.UTF8.GetBytes( //This is responsible for creating a new symmetric security key using the secret key defined in the appsettings.json file in Identity API Core
            //                        builder.Configuration["AppSettings:JWTSecret"]!) //This is responsible for getting the secret key from the appsettings.json file using the Configuration object in Identity API Core
            //                    ) 

            //            };

            //        });
            #endregion

            //New Code Using Extension Method for Adding Authentication and JWT Bearer Token Validation
            builder.Services.AddIdentityAuth(builder.Configuration);
            //===================================================================================================


            //Defining the AppSettings class to hold the configuration values from the appsettings.json file
            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings")); //This is responsible for binding the AppSettings section of the appsettings.json file to the AppSettings class in Identity API Core


            var app = builder.Build();
            //===================================================================================================
            // Configure the HTTP request pipeline.
            //<--Old Code-->
            #region Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}
            #endregion

            //New Code Using Extension Method for Adding Swagger/OpenAPI Middleware
            app.ConfigureSwaggerExplorer();
            //===================================================================================================


            app.UseHttpsRedirection();


            //===================================================================================================
            //Adding the CORS policy to allow requests from the frontend application
            #region Config. CORS
            //app.UseCors(options => 
            //    options.WithOrigins("http://localhost:4200")
            //    .AllowAnyMethod()
            //    .AllowAnyHeader());
            #endregion

            app.CORSConfig(builder.Configuration);//Adding extra parameter for IConfiguration to get the CORS settings from appsettings.json
            //===================================================================================================


            //===================================================================================================
            //This is responsible for adding the authentication middleware to the HTTP request pipeline.
            //It should be added before the authorization middleware.
            //<--Old Code-->
            #region AddAuthentication and Authorization Middleware
            //app.UseAuthentication(); 

            //app.UseAuthorization();
            #endregion

            app.AddIdentityAuthMiddlewares();
            //===================================================================================================


            app.MapControllers();

            //Adding the respective Routes for Identity API Core
            app
                .MapGroup("/api") //This is responsible for grouping the endpoints for Identity API Core under the "/api" route
                .MapIdentityApi<AppUser>(); //This is responsible for mapping the endpoints for Identity API Core
                                            //The IdentityUser is the default user class provided by ASP.NET Core Identity.
                                            //It includes properties like UserName, Email, PasswordHash, etc.
                                            //You can also create a custom user class that inherits from IdentityUser
                                            //if you need to add additional properties specific to your application.


            //===================================================================================================
            //Using the MapIdentityUserEndpoints extension method that was defined inside the IdentityUserEndpoints class to map the endpoints for Identity API Core        
            app.MapIdentityUserEndpoints(builder.Configuration);

            //<--Old Code-->
            #region Minimal API Controllers for Identity API Core
            //Using Minimal API Controllers for Identity API Core
            //app.MapPost("/api/signup", async(
            //    UserManager<AppUser> userManager, //This is responsible for managing the users in Identity API Core
            //    [FromBody] UserRegistrationModel userRegistrationModel //This is responsible for binding the data from the request body to the UserRegistrationModel class
            //    ) => 
            //{
            //    //Creating a new user object and populating it with the data from the request body
            //    AppUser user = new AppUser()
            //    {
            //        UserName = userRegistrationModel.Email, //Setting the UserName property to the email address of the user
            //        Email = userRegistrationModel.Email,
            //        FullName = userRegistrationModel.FullName
            //    };
            //    //The UserManager class provides various methods for managing users, such as creating, updating, deleting, etc.
            //    var result = await userManager.CreateAsync(
            //        user, 
            //        userRegistrationModel.Password); //This is responsible for creating a new user in Identity API Core
            //                                         //The CreateAsync method takes the user object and the password as parameters and creates a new user in the database.
            //                                         //It returns a Task<IdentityResult> which indicates whether the operation was successful or not.
            //                                         //You can also handle the result of the operation to return appropriate responses to the client.

            //    //returning the result of the operation to the client
            //    if (result.Succeeded)
            //    {
            //        return Results.Ok(result);
            //    }
            //    else
            //    {
            //        return Results.BadRequest(result.Errors);
            //    }
            //});

            ////Adding the SignIn endpoint for Identity API Core to authenticate users and generate JWT tokens for them to access protected resources in the application.
            //app.MapPost("/api/signin", async(
            //        UserManager<AppUser> userManager, 
            //        [FromBody] UserLoginModel userLoginModel
            //    ) =>
            //{
            //    var user = await userManager.FindByEmailAsync(userLoginModel.Email); //With the help of UserManager, we will finr if there is a user with the given credentials in the database or not.

            //    //Making sure that there is a user with the given email address.
            //    if(user != null && await userManager.CheckPasswordAsync(user, userLoginModel.Password)) //This method will check if the password provided in the request body matches the password of the user found in the database.
            //    {   // Generate JWT token
            //        var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
            //                                (
            //                                    builder.Configuration["AppSettings:JWTSecret"]!
            //                                )
            //                            );

            //        //Creating Claims or Payload for the JWT token
            //        var tokenDescriptor = new SecurityTokenDescriptor
            //        {
            //            Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
            //            {
            //                new Claim("UserID", user.Id.ToString())
            //            }),
            //            Expires = DateTime.UtcNow.AddDays(10), //Setting the expiration time for the token to 10 days
            //            SigningCredentials = new SigningCredentials(
            //                signInKey,
            //                SecurityAlgorithms.HmacSha256Signature //This is responsible for specifying the algorithm used to sign the token in Identity API Core
            //                )
            //        };

            //        var tokenHandler = new JwtSecurityTokenHandler(); //This is responsible for creating and validating JWT tokens in Identity API Core
            //        var securityToken = tokenHandler.CreateToken(tokenDescriptor); //This is responsible for creating a new JWT token using the token descriptor defined above in Identity API Core
            //        var token = tokenHandler.WriteToken(securityToken); //This is responsible for writing the JWT token to a string format that can be returned to the client in Identity API Core)
            //        return Results.Ok(new { token }); //This is responsible for returning the JWT token to the client along with the user ID and full name of the user in Identity API Core
            //                                          //The client can then use this token to access protected resources in the application by including it in the Authorization header of the requests.
            //    }
            //    else
            //    {
            //        return Results.BadRequest(new {message = "Username or Password is incorrect."});
            //    }
            //});
            #endregion



            app.Run();
        
        }
    }
}
