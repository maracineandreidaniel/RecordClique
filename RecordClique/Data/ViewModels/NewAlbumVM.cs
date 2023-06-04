using RecordClique.Data.Base;
using RecordClique.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecordClique.Models
{
    public class NewAlbumVM
    {
        public int Id { get; set; } 

        [Display(Name = "Album Name")]
        [Required(ErrorMessage = "Name is required!")]
        public string AlbumName { get; set; }


        [Display(Name = "Album Description")]
        [Required(ErrorMessage = "Name is required!")]
        public string AlbumDescription { get; set; }


        [Display(Name = "Album Price")]
        [Required(ErrorMessage = "Price is required!")]
        public int AlbumPrice { get; set; }

        [Display(Name = "Album Cover")]
        [Required(ErrorMessage = "Album Cover is required!")]
        public string AlbumCoverURL { get; set; }


        [Display(Name = "Release Date")]
        [Required(ErrorMessage = "Release Date is required!")]
        public DateTime AlbumReleaseDate { get; set; }       

        [Display(Name = "Genre")]        
        public AlbumGenre AlbumGenre { get; set; }

        [Display(Name = "Select artist/artists")]
        [Required(ErrorMessage = "Actors are required!")]
        public List<int> ArtistIds { get; set; }


        [Display(Name = "Label")]
        [Required(ErrorMessage = "Label is required!")]
        public int LabelID { get; set; }

        

        



    }

}
