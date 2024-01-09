using System.ComponentModel.DataAnnotations;

namespace Symphony_LTD.Models
{
    public class About
    {
        [Key]
        public int Id { get; set; }
        
        public string? ImageOne { get; set; }
        
        public string? ImageTwo { get; set; }
        
        public string? ParagraphOne {  get; set; }
        
        public string? ParagraphTwo {  get; set; }
        
        public string? ParagraphThree {  get; set; }
       
        public string? ParagraphFour {  get; set; }
        
        public string? HeadingOne {  get; set; }

        public string? HeadingTwo {  get; set; }
        


    }
}
