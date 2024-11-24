using System.ComponentModel.DataAnnotations;

namespace _1111760.Models
{
    public class User
    {
        [Required]
        [Display(Name = "品牌編號")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "品牌名稱")]
        public string Name { get; set; }

        
        [Display(Name = "源自國家")]
        public string Country { get; set; }

        
        [Display(Name = "成立年份")]
        public int Year { get; set; }

        
        [Display(Name = "品牌類別")]
        public string Type { get; set; }

    }
}
