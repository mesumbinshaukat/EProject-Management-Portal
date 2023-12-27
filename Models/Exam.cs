using System.ComponentModel.DataAnnotations;

namespace Symphony_LTD.Models
{
    public class Exam
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime ExamDate { get; set; }
        [Required]
        public decimal Score { get; set; }
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int CourseId { get; set; }
       
    }
}
