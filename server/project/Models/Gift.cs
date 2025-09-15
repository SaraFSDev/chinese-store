using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace project.Models
{
    public class Gift
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [MinLength(2, ErrorMessage = "MinLength is 2")]
        [MaxLength(50, ErrorMessage = "MaxLength is 50")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(10, 150, ErrorMessage = "Price range between 10 and 150")]
        public int Price { get; set; }
        public string? image { get; set; }
        public string Status { get; set; } = "Draft";

        public int? CategoryId { get; set; }
        public int? DonatorId { get; set; }
        public int? WinningId { get; set; }

        [JsonIgnore]
        public ICollection<UserGift> UserGifts { get; set; } = new List<UserGift>();

        public virtual Category? Category { get; set; }
        public virtual Donator? Donator { get; set; }
        public virtual Winning? Winning { get; set; }
        public bool IsLotteryCompleted { get; set; } = false;

    }
}



