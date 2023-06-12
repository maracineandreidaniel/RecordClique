//using RecordClique.Models;

//namespace RecordClique.Data.Extensions
//{
//    public static class NewsFeedLogExtensions
//    {
//        public static string DisplayAction(this NewsFeedLog log)
//        {
//            string message = "";

//            switch (log.Action)
//            {
//                case 1:
//                    message = $"{log.UserName} started friendship with {log.Friend}";
//                    break;
//                case 2:
//                    message = $"{log.UserName} ended friendship with {log.Friend}";
//                    break;
//                case 3:
//                    message = $"{log.UserName} added to favourites the album {log.Album}";
//                    break;
//                case 4:
//                    message = $"{log.UserName} removed from favourites the album {log.Album}";
//                    break;
//            }

//            return message;
//        }
//    }

//}


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
                    message = $"{user?.FullName ?? log.UserName} added to favourites the album {log.Album}";
                    break;
                case 4:
                    message = $"{user?.FullName ?? log.UserName} removed from favourites the album {log.Album}";
                    break;
            }

            return message;
        }
    }
}

