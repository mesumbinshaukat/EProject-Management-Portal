using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Symphony_LTD.Models
{
    public class Result
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int? StudentId { get; set; }
        [Required]
        public int? SubjectId { get; set; }
        [Required]
        [DisplayName("Comments")]
        public string? Comments { get; set; }
        [Required]
        public decimal? MarksObtained { get; set; }
    }
}
