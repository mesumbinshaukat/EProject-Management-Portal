using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Symphony_LTD.Models
{
    public class About
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("First Image")]
        public string? ImageOne { get; set; }
        [Required]
        [DisplayName("Second Image")]
        public string? ImageTwo { get; set; }
        [Required]
        [DisplayName("First Paragraph")]
        public string? ParagraphOne {  get; set; }
        [Required]
        [DisplayName("Second Paragraph")]
        public string? ParagraphTwo {  get; set; }
        [Required]
        [DisplayName("Third Paragraph")]
        public string? ParagraphThree {  get; set; }
        [Required]
        [DisplayName("Fourth Paragraph")]
        public string? ParagraphFour {  get; set; }
        [Required]
        [DisplayName("First Heading")]
        public string? HeadingOne {  get; set; }
        [Required]
        [DisplayName("Second Heading")]
        public string? HeadingTwo {  get; set; }
        


    }
}
