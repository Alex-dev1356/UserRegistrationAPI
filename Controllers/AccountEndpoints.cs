using AuthECAPI.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AuthECAPI.Controllers
{
    public static class AccountEndpoints
    {
        public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder app, IConfiguration config)
        {
            //Here we're going to add Web API Methods related to Account Management, such as GetUserProfile, UpdateUserProfile, DeleteUserProfile, etc.
            app.MapGet("/api/UserProfile", GetUserProfile)
                .RequireAuthorization(); //We added the RequireAuthorization() method to the endpoint to ensure that only authenticated users can access this endpoint.

            return app;
        }

        private static async Task<IResult> GetUserProfile(ClaimsPrincipal user, //This method is responsible for retrieving the user profile information of the authenticated user. The ClaimsPrincipal parameter represents the current authenticated user and contains their claims, which can be used to retrieve their information.
            UserManager<AppUser> userManager) 
        {
            string userID = user.Claims.First(x => x.Type == "UserID").Value;
            var userDetails =await userManager.FindByIdAsync(userID);

            //return "User Profile";

            return Results.Ok(
                new 
                { 
                    Email = userDetails?.Email,
                    FullName = userDetails?.FullName,
                });

        }
    }
}
