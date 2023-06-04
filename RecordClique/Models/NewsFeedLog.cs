namespace RecordClique.Models
{
    public class NewsFeedLog
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Action { get; set; }
        public string Friend { get; set; }
        public string Album { get; set; }
    }

}
