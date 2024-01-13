﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Symphony_LTD.Models
{
    public class CourseExamResult
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Exam Name")]
        public int? ExamName { get; set; }
        [Required]
        [DisplayName("Student Name")]
        public int? StudentId { get; set; }
        [Required]
        [DisplayName("Course Exam")]
        public int? Course { get; set; }
        [Required]
        [DisplayName("Comments")]
        public string? Comments { get; set; }
        [Required]
        [DisplayName("Total Marks")]
        public string? TotalMarks { get; set; }
        [Required]
        [DisplayName("Marks Obtained")]
        public decimal? MarksObtained { get; set; }               
        [Required]
        [DisplayName("Class")]
        public int? Class { get; set; }
        [Required]
        [DisplayName("Date")]
        public DateTime? Date { get; set; }

    }
}
