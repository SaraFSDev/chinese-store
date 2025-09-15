using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace project.Models
{
    public class Donator
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MinLength(2, ErrorMessage = "MinLength is 2")]
        [MaxLength(50, ErrorMessage = "MaxLength is 20")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Phone is type phoneNumber")]
        public string? Phone { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "email is type EmailAddress")]
        public string? Email { get; set; }
        public ICollection<Gift>? Gifts { get; set; }= new List<Gift>();
    }
}
