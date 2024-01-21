using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Symphony_LTD.Models
{
    public class EntranceExamList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Entrance Exam Name")]
        public string? Name { get; set; }
        [Required]
        [DisplayName("Total Score/Marks")]
        public int? Marks { get; set; }
        [DisplayName("Entrance Exam Availablity")]
        [DefaultValue(true)]
        public bool? Availablity { get; set; } = true;
        [Required]
        [DisplayName("Exam Description")]
        public string? Description { get; set; }
        [DisplayName("Exam Schedule Date")]
        public DateTime? Date { get; set; }

    }
}
