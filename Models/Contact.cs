using System.ComponentModel.DataAnnotations;

namespace Symphony_LTD.Models
{
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string CenterName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Message { get; set; } 
    }
}
