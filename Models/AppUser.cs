using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthECAPI.Models
{
    public class AppUser : IdentityUser
    {
        [PersonalData]//Adding PersonalData attribute to the FullName property to indicate that it contains personal data of the user.
        //Addin the data type of the FullName property to be nvarchar(150) in the database
        [Column(TypeName = "nvarchar(150)")]
        public string FullName { get; set; } = string.Empty; //Adding the FullName property to the AppUser class.
                                                             //This property will store the full name of the user and is required (not nullable).
    }
}
