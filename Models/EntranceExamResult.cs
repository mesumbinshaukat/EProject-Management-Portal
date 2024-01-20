using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Symphony_LTD.Models
{
    public class EntranceExamResult
    {
        [Key]
        public int Id { get; set; }        
        [Required]
        [DisplayName("Student Name")]
        public int? StudentId { get; set; }
        
        [DisplayName("Course Exam")]
        public int? Course { get; set; }
        [Required]
        [DisplayName("Comments")]
        public string? Comments { get; set; }        
        [Required]
        [DisplayName("Marks Obtained")]
        public decimal? MarksObtained { get; set; }
    }
}
