using System.ComponentModel.DataAnnotations;

namespace project.Models.ModelsDto
{
    public class GiftDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string image { get; set; }
        public string? DonatorName { get; set; }
        public int? DonatorId { get; set; }
        public string? Category { get; set; }
        public int? CategoryId { get; set; }
        public int Purchases { get; set; }
        public bool IsLotteryCompleted { get; set; }

    }
}



