using RecordClique.Data;
using RecordClique.Models;
using System.Linq;

namespace RecordClique.Data.Extensions
{
    public static class NewsFeedLogExtensions
    {
        public static string DisplayAction(this NewsFeedLog log, AppDbContext context)
        {
            var user = context.Users.FirstOrDefault(u => u.Id == log.UserName);
            var friend = context.Users.FirstOrDefault(u => u.Id == log.Friend);
            var album = context.Albums.FirstOrDefault(u => u.Id.ToString() == log.Album);
            string message = "";

            switch (log.Action)
            {
                case 1:
                    message = $"{user?.FullName ?? log.UserName} started friendship with {friend?.FullName ?? log.Friend}";
                    break;
                case 2:
                    message = $"{user?.FullName ?? log.UserName} ended friendship with {friend?.FullName ?? log.Friend}";
                    break;
                case 3:
                    message = $"{user?.FullName ?? log.UserName} added to favourites the album {album?.AlbumName}";
                    break;
                case 4:
                    message = $"{user?.FullName ?? log.UserName} removed from favourites the album {album?.AlbumName}";
                    break;
                case 5:
                    message = $"{user?.FullName ?? log.UserName} added to Wishlist the album {album?.AlbumName}";
                    break;
                case 6:
                    message = $"{user?.FullName ?? log.UserName} removed from Wishlist the album {album?.AlbumName}";
                    break;
                case 7:
                    message = $"{user?.FullName ?? log.UserName} added to Listening the album {album?.AlbumName}";
                    break;
                case 8:
                    message = $"{user?.FullName ?? log.UserName} removed from Listening the album {album?.AlbumName}";
                    break;
            }

            return message;
        }
    }
}

