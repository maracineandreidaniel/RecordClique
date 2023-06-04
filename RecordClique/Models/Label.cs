using RecordClique.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace RecordClique.Models
{
    public class Label: IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Profile Picture")]
        [Required(ErrorMessage ="Profile Picture Required")]
        public string ProfilePictureURL { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name Required")]
        public string LabelName { get; set; }

        [Display(Name = "Biography")]
        public string LabelBio { get; set;}

        public List<Album> Albums { get; set; }
    }
}
