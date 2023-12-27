using System.ComponentModel.DataAnnotations;

namespace Symphony_LTD.Models
{
    public class Course
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string CourseName { get; set; }
        [Required]
        public string TopicsCovered { get; set; }
        [Required]
        public decimal CourseFee { get; set; }
        [Required]
        public string CourseDetails { get; set; }
        [Required]
        public string EntranceExamDetails { get; set; }

    }
}
