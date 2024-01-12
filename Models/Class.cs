using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Symphony_LTD.Models
{
    public class Class
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [DisplayName("Class")]
        public int? _Class { get; set; }
        [Required]
        [DisplayName("Class Strength / Student Limit")]
        public double? Limit { get; set; }
        [Required]
        [DisplayName("Course")]
        public int? Course { get; set; }
    }
}
