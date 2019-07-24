using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GestorInventarioEmpresas.BackEnd.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
     [JsonIgnore]
        public virtual UserProfile UserProfile { get; set; }
        public string DisplayName
        {
            get
            {
                var name = UserProfile?.Name?.Trim();
                return !string.IsNullOrEmpty(name) ? name : UserName;
            }
        }
    }
}
