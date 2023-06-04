using RecordClique.Models;

namespace RecordClique.Data.Extensions
{
    public static class NewsFeedLogExtensions
    {
        public static string DisplayAction(this NewsFeedLog log)
        {
            string message = "";

            switch (log.Action)
            {
                case 1:
                    message = $"{log.UserName} started friendship with {log.Friend}";
                    break;
                case 2:
                    message = $"{log.UserName} ended friendship with {log.Friend}";
                    break;
                case 3:
                    message = $"{log.UserName} added to favourites the album {log.Album}";
                    break;
                case 4:
                    message = $"{log.UserName} removed from favourites the album {log.Album}";
                    break;
            }

            return message;
        }
    }

}
