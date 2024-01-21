using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Symphony_LTD.Models
{
    public class EntranceExam
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Student")]
        public int? StudentId { get; set; }
        
        [DisplayName("Exam Name")]
        public string? ExamName { get; set; }
        
        [DisplayName("Total Marks")]
        public int? TotalMarks { get; set; }
        [Required]
        [DisplayName("Course Exam")]
        public int? Course {  get; set; }
        
        [DisplayName("Exam Description")]
        public string? Description { get; set; }
        
        [DisplayName("Date")]
        public DateTime? Date { get; set; }
        [DisplayName("Status")]
        [DefaultValue(true)]
        public bool? Pending { get; set; } = true;
        
    }
}
