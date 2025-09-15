using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace project.Models
{
    public class User : IdentityUser
    {
        [JsonIgnore]
        public ICollection<UserGift> UserGifts { get; set; } = new List<UserGift>();
    }
}
