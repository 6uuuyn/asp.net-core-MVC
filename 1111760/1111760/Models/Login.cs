using System.ComponentModel.DataAnnotations;

namespace _1111760.Models
{
    public class Login
    {
        [Key]
        public int Number { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
