using RecordClique.Models;

public class UserAlbumsVM
{
    public string FriendName { get; set; }
    public IEnumerable<Album> FavouriteAlbums { get; set; }
    public IEnumerable<Album> WishlistAlbums { get; set; }
    public IEnumerable<Album> ListeningAlbums { get; set; }
}
