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
        [DisplayName("Course Exam")]
        public int? Course {  get; set; }
        [Required]
        [DisplayName("Exam Description")]
        public string? Description { get; set; }
        [Required]
        [DisplayName("Date")]
        public DateTime? Date { get; set; }
        [DisplayName("Status")]
        [DefaultValue(true)]
        public bool? Pending { get; set; } = true;
    }
}
