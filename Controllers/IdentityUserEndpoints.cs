using AuthECAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthECAPI.Controllers
{
    public static class IdentityUserEndpoints
    {
        //Here we're going to add Web API Methods related to Identity User Management, such as Register, Login, GetUser, UpdateUser, DeleteUser, etc.
        public static IEndpointRouteBuilder MapIdentityUserEndpoints(this IEndpointRouteBuilder app, IConfiguration config)
        {
            //Utilizing the CreateUser method that we defined to create a new user in the database and return the result of the operation to the client.
            app.MapPost("/api/signup", CreateUser);

            //Adding the SignIn endpoint for Identity API Core to authenticate users and generate JWT tokens for them to access protected resources in the application.
            app.MapPost("/api/signin", Signin);

            return app;
        }

        //Defining the CreateUser method that will be responsible for creating a new user in the database and returning the result of the operation to the client.
        public static async Task<IResult> CreateUser(
                UserManager<AppUser> userManager, //This is responsible for managing the users in Identity API Core
                [FromBody] UserRegistrationModel userRegistrationModel //This is responsible for binding the data from the request body to the UserRegistrationModel class
                )
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
        }


        public static async Task<IResult> Signin(
                UserManager<AppUser> userManager,
                [FromBody] UserLoginModel userLoginModel,
                IOptions<AppSettings> appSettings)
        {
            var user = await userManager.FindByEmailAsync(userLoginModel.Email); //With the help of UserManager, we will finr if there is a user with the given credentials in the database or not.

            //Making sure that there is a user with the given email address.
            if (user != null && await userManager.CheckPasswordAsync(user, userLoginModel.Password)) //This method will check if the password provided in the request body matches the password of the user found in the database.
            {   // Generate JWT token
                var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                                        (
                                            //builder.Configuration["AppSettings:JWTSecret"]!
                                            appSettings.Value.JWTSecret
                                        )
                                    );

                //Creating Claims or Payload for the JWT token
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                    {
                            new Claim("UserID", user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(10), //Setting the expiration time for the token to 10 days
                    SigningCredentials = new SigningCredentials(
                        signInKey,
                        SecurityAlgorithms.HmacSha256Signature //This is responsible for specifying the algorithm used to sign the token in Identity API Core
                        )
                };

                var tokenHandler = new JwtSecurityTokenHandler(); //This is responsible for creating and validating JWT tokens in Identity API Core
                var securityToken = tokenHandler.CreateToken(tokenDescriptor); //This is responsible for creating a new JWT token using the token descriptor defined above in Identity API Core
                var token = tokenHandler.WriteToken(securityToken); //This is responsible for writing the JWT token to a string format that can be returned to the client in Identity API Core)
                return Results.Ok(new { token }); //This is responsible for returning the JWT token to the client along with the user ID and full name of the user in Identity API Core
                                                  //The client can then use this token to access protected resources in the application by including it in the Authorization header of the requests.
            }
            else
            {
                return Results.BadRequest(new { message = "Username or Password is incorrect." });
            }
        }
    }
}
