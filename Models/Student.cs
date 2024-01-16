using Org.BouncyCastle.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Symphony_LTD.Models
{
    public class Student
    {
        [Key]
        public int? StudentId { get; set; }
        [Required]
        [DisplayName("Roll Number")]
        public int? RollNumber { get; set; }
       
        [DisplayName("First Name")]
        public string? FirstName { get; set; }
        [DisplayName("Middle Name (Optional)")]
        public string? MiddleName {  get; set; } = null;
        [Required]
        [DisplayName("Last Name")]
        public string? LastName { get; set; }
        [Required]
        [DisplayName("Date Of Birth")]
        public DateTime? DateOfBirth { get; set; }
        [Required]
        [DisplayName("Address")]
        public string? Address { get; set; }
        [Required]
        [DisplayName("Email")]
        public string? Email { get; set; }
        [Required]
        [DisplayName("Phone Number")]
        public string? PhoneNumber { get; set; }
        [DisplayName("Picture")]
        public string? Picture { get; set; }
        [DisplayName("Accept")]
        [DefaultValue(null)]
        public string? Accept { get; set; }
        [DisplayName("Password")]
        [DefaultValue(null)]
        public string? Password { get; set; }
        [DisplayName("Class")]
        [DefaultValue(null)]
        public int? Class { get; set;}
    }
}
