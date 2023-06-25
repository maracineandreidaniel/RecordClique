using RecordClique.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RecordClique.Data
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

           


            modelBuilder.Entity<Friendship>()
            .HasKey(f => new { f.InitiatorId, f.FriendId });

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.Initiator)
                .WithMany(u => u.FriendshipsInitiated)
                .HasForeignKey(f => f.InitiatorId)
                .OnDelete(DeleteBehavior.Restrict);  

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.Friend)
                .WithMany(u => u.FriendshipsAccepted)
                .HasForeignKey(f => f.FriendId)
                .OnDelete(DeleteBehavior.Restrict);  

            modelBuilder.Entity<UserAlbum>()
            .HasKey(ua => new { ua.ApplicationUserId, ua.AlbumId });

            modelBuilder.Entity<UserAlbum>()
                .HasOne(ua => ua.ApplicationUser)
                .WithMany(u => u.UserAlbums)
                .HasForeignKey(ua => ua.ApplicationUserId);

            modelBuilder.Entity<UserAlbum>()
                .HasOne(ua => ua.Album)
                .WithMany(a => a.UserAlbums)
                .HasForeignKey(ua => ua.AlbumId);


           

            base.OnModelCreating(modelBuilder);

        }

        public DbSet<Album> Albums { get;set; }

        public DbSet<Artist> Artists { get;set; }
        

        public DbSet<Label> Labels { get;set; }       

        public DbSet<Friendship> Friendships { get; set; }

        public DbSet<UserAlbum> UserAlbums { get; set; }


        public DbSet<NewsFeedLog> NewsFeedLogs { get; set; }

        public DbSet<Comment> Comments { get; set; }





    }
}
