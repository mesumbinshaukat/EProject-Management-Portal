using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Symphony_LTD.Models
{
    public class Exam
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Exam Name")]
        public string? ExamName { get; set; }
        [DisplayName("Date")]
        public DateTime? ExamDate { get; set; }
        [Required]
        [DisplayName("Score")]
        public decimal? Score { get; set; }
        [Required]
        [DisplayName ("Student Roll No")]
        public int? StudentId { get; set; }
        [Required]
        [DisplayName("Course Name")]
        public int? CourseId { get; set; }
        [Required]
        [DisplayName("Exam Detail")]
        public string? Detail {  get; set; }
        [DisplayName("Status")]
        [DefaultValue(true)]
        public bool? Pending { get; set; } = true;
    }
}
