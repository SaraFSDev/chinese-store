using System.ComponentModel.DataAnnotations;

namespace project.Models
{
    public class Winning
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? LinkedUserId { get; set; }
        public virtual User? LinkedUser { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;




    }

}

