using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Symphony_LTD.Models
{
    public class CourseExam
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Exam Name")]
        public string? ExamName { get; set; }
        [Required]
        [DisplayName("Class")]
        public int? Class {  get; set; }
        [Required]
        [DisplayName("Total Score")]
        public float? TotalScore { get; set; }   
        [Required]
        [DisplayName("Description")]
        public string? Description { get; set; }
        
        [DisplayName("Date")]
        public DateTime? Date { get; set; }

        [DisplayName("Status")]
        [DefaultValue(true)]
        public bool? Pending { get; set; } = true;
    }
}
