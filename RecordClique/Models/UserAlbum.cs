using RecordClique.Models;

public class UserAlbum
{
    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    public int AlbumId { get; set; }
    public Album Album { get; set; }

    public bool IsListening { get; set; }
    public bool IsFavourite { get; set; }
    public bool IsOnWishlist { get; set; }
}
