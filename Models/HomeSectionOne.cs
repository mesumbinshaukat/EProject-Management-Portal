using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Symphony_LTD.Models
{
    public class HomeSectionOne
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Heading 5 - <h5></h5>")]
        public string? H5 { get; set; }
        [DisplayName("Heading 2 - <h2></h2>")]
        public string? H2 { get; set; }
        [DisplayName("Paragraph")]
        public string? Paragraph { get; set;}
        [DisplayName("Button Value")]
        public string? BtnVal { get; set; }
        [DisplayName("Button HREF/Action/Redirection/Url")]
        public string? BtnAction { get; set; }
        [DisplayName("Image")]
        public string? Img { get; set; }

    }
}
