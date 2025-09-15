using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.Models.ModelsDTO
{
    public class UserGiftDTO
    {
        public int GiftId { get; set; }
        public string GiftName { get; set; }
        public string GiftImage { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public int GiftPrice { get; set; }
        public int Quantity { get; set; }

    }
}
