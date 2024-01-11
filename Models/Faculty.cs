using System.ComponentModel.DataAnnotations;

namespace Symphony_LTD.Models
{
    public class Faculty
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Role { get; set; }
        
        public string? Image {  get; set; }
    }
}
