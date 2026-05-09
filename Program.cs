
using AuthECAPI.Models;
using Microsoft.AspNetCore.Identity;

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
                .AddIdentityApiEndpoints<IdentityUser>()//This is responsible for adding the endpoints for Identity API Core
                                                        //The IdentityUser is the default user class provided by ASP.NET Core Identity.
                                                        //It includes properties like UserName, Email, PasswordHash, etc.
                                                        //You can also create a custom user class that inherits from IdentityUser
                                                        //if you need to add additional properties specific to your application.
            .AddEntityFrameworkStores<ApplicationDbContext>(); //This is responsible for adding the stores for Identity API Core
                                                                //The ApplicationDbContext is the database context that will be used to store the identity data.
                                                                //You need to create this class and configure it to use your database provider (e.g., SQL Server, SQLite, etc.)
                                                                //It should inherit from IdentityDbContext<IdentityUser> or a custom user class if you created one.

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
