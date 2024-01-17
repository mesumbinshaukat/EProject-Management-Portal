using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Symphony_LTD.Models
{
    public class Course
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [DisplayName("Course Name")]
        public string? CourseName { get; set; }
        [Required]
        [DisplayName("Topics Covered")]
        public string? TopicsCovered { get; set; }
        [Required]
        [DisplayName("Course Fee")]
        public decimal? CourseFee { get; set; }
        [Required]
        [DisplayName("Course Details")]
        public string? CourseDetails { get; set; }
        

    }
}
