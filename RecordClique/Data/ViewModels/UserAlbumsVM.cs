using RecordClique.Models;

namespace RecordClique.Data.ViewModels
{
    public class UserAlbumsVM
    {
        public IEnumerable<Album> FavouriteAlbums { get; set; }
        public IEnumerable<Album> WishlistAlbums { get; set; }
        public IEnumerable<Album> ListeningAlbums { get; set; }
    }

}
