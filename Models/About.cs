using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Symphony_LTD.Models
{
    public class About
    {
        [Key]
        public int Id { get; set; }
        
        [DisplayName("First Image")]
        public string? ImageOne { get; set; }
        
        [DisplayName("Second Image")]
        public string? ImageTwo { get; set; }
        
        [DisplayName("First Paragraph")]
        public string? ParagraphOne {  get; set; }
       
        [DisplayName("Second Paragraph")]
        public string? ParagraphTwo {  get; set; }
       
        [DisplayName("Third Paragraph")]
        public string? ParagraphThree {  get; set; }
        
        [DisplayName("Fourth Paragraph")]
        public string? ParagraphFour {  get; set; }
        
        [DisplayName("First Heading")]
        public string? HeadingOne {  get; set; }
        
        [DisplayName("Second Heading")]
        public string? HeadingTwo {  get; set; }
        


    }
}
