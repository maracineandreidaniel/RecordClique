using RecordClique.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace RecordClique.Models
{
    public class Artist:IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Profile Picture")]
        [Required(ErrorMessage = "Profile picture is required!")]
        public string ProfilePictureURL { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required!")]
        [StringLength(10,MinimumLength = 3,ErrorMessage = "Full name between 3 and 10")]
        public string ArtistName { get; set; }

        [Display(Name = "Biography")]
        [Required(ErrorMessage = "Biography is required!")]
        public string ArtistBio { get; set;}


        public List<Album> Albums { get; set; }


    }
}
