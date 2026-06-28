namespace AuthECAPI.Models
{
    public class AppSettings
    {
        //Adding all the properties that we want to read from the appsettings.json file in this class, so that we can use them in our application.
        public string JWTSecret { get; set; } = string.Empty; //This is responsible for storing the secret key that will be used to sign the JWT tokens in Identity API Core

    }
}
