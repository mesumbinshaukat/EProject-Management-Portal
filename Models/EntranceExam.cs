using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Symphony_LTD.Models
{
    public class EntranceExam
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Exam Name")]
        public string? ExamName { get; set; }
        [Required]
        [DisplayName("Total Marks")]
        public string? TotalMarks { get; set; }
        [Required]
        [DisplayName("Courses Will Covered")]
        public int[]? Courses {  get; set; }
        [Required]
        [DisplayName("Exam Description")]
        public string? Description { get; set; }
    }
}
