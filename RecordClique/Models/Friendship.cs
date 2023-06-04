namespace RecordClique.Models
{
    public class Friendship
    {
        public string InitiatorId { get; set; }
        public ApplicationUser Initiator { get; set; }

        public string FriendId { get; set; }
        public ApplicationUser Friend { get; set; }
    }
}
