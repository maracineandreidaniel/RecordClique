using RecordClique.Data.Base;
using RecordClique.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecordClique.Models
{
    public class Album : IEntityBase
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "Name")]
        public string AlbumName { get; set; }

        [Display(Name = "Description")]
        public string AlbumDescription { get; set; }

        public int AlbumPrice { get; set; }

        [Display(Name = "Cover")]
        public string AlbumCoverURL  { get; set; }

        public DateTime AlbumReleaseDate { get; set; }       


        public string AlbumGenre { get; set; } 
        

        public int LabelID { get; set; }

        [ForeignKey("LabelID")]
        public Label Label { get; set; }      


        public int ArtistId { get; set; }

        [ForeignKey("ArtistId")]
        public Artist Artist { get; set; }


        public ICollection<UserAlbum> UserAlbums { get; set; }
    }

}
