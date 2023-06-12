using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecordClique.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Display(Name = "Full name")]
        public string FullName { get; set; }





        public ICollection<Friendship> FriendshipsInitiated { get; set; }
        public ICollection<Friendship> FriendshipsAccepted { get; set; }

        public ICollection<UserAlbum> UserAlbums { get; set; }




    }
}
