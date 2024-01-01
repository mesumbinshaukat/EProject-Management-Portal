using System.ComponentModel.DataAnnotations;

namespace Symphony_LTD.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(40, MinimumLength = 4)]
        public string? Name { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required, StringLength(70, MinimumLength = 6)]
        public string? Password { get; set; }
    }
}
