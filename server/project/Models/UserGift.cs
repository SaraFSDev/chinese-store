using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;


namespace project.Models
{
    public class UserGift
    {

        public string UserId { get; set; }
        public User User { get; set; }

        public int GiftId { get; set; }
        [JsonIgnore]
        public Gift Gift { get; set; }
        public string GiftStatus { get; set; } = "Draft";

        public int Quantity { get; set; }
    }
}
