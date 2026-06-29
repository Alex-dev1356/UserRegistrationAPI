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

        private static string GetUserProfile()
        {
            return "User Profile";
        }
    }
}
