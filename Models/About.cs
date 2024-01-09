using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Symphony_LTD.Models
{
    public class About
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("First Image (Optional)")]
        public string? ImageOne { get; set; }
        [DisplayName("Second Image (Optional)")]
        public string? ImageTwo { get; set; }
        [DisplayName("First Paragraph (Optional)")]
        public string? ParagraphOne {  get; set; }
        [DisplayName("Second Paragraph (Optional)")]
        public string? ParagraphTwo {  get; set; }
        [DisplayName("Third Paragraph (Optional)")]
        public string? ParagraphThree {  get; set; }
        [DisplayName("Fourth Paragraph (Optional)")]
        public string? ParagraphFour {  get; set; }
        [DisplayName("First Heading (Optional)")]
        public string? HeadingOne {  get; set; }
        [DisplayName("Second Heading (Optional)")]
        public string? HeadingTwo {  get; set; }
        


    }
}
