using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Symphony_LTD.Models
{
    public class Branch
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Branch Name")]
        public string? Branches { get; set; }
        [Required]
        [DisplayName("Branch Code")]
        public string? Code { get; set; }
        [Required]
        [DisplayName("Address")]
        public string? Address { get; set; }
        [Required]
        [DisplayName("City")]
        public string? City { get; set; }
        [Required]
        [DisplayName("Country")]
        public string? Country { get; set; }
        [Required]
        [DisplayName("Zip/Postal Code")]
        public  string? Postal { get; set; }
    }
}
