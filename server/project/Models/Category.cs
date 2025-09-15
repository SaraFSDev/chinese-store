using System.ComponentModel.DataAnnotations;

namespace project.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MinLength(2, ErrorMessage = "MinLength is 2")]
        [MaxLength(50, ErrorMessage = "MaxLength is 20")]
        public string name { get; set; }
    }
}
