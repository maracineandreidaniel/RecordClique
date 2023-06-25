using RecordClique.Data;
using RecordClique.Data;
using RecordClique.Data.ViewModels;
using RecordClique.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RecordClique.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }


        public async Task<IActionResult> Users()
        {
            try
            {
                var currentUserId = _userManager.GetUserId(User);
                var users = await _context.Users.Where(u => u.Id != currentUserId).ToListAsync();
                return View(users);
            }
            catch (Exception ex)
            {                
                return RedirectToAction("AlreadyAddedFriend.cshtml", "Errors");
            }
        }






        public IActionResult Login() => View(new LoginVM());

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if(user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "NewsFeed");
                    }
                }
                TempData["Error"] = "Wrong credentials. Please, try again!";
                return View(loginVM);
            }

            TempData["Error"] = "Wrong credentials. Please, try again!";
            return View(loginVM);
        }


        public IActionResult Register() => View(new RegisterVM());

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if(user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerVM);
            }

            var newUser = new ApplicationUser()
            {
                FullName = registerVM.FullName,
                Email = registerVM.EmailAddress,
                UserName = registerVM.EmailAddress
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

  

            return View("RegisterCompleted");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "NewsFeed");
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            return View();
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyFriends()
        {
            var user = await _userManager.GetUserAsync(User);
            var friends = _context.Friendships
                .Where(f => f.InitiatorId == user.Id)
                .Select(f => f.InitiatorId == user.Id ? f.Friend : f.Initiator)
                .ToList();
            return View(friends);
        }


        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddToFavourite(int albumId)
        {
            var user = await _userManager.GetUserAsync(User);
            var userAlbum = await _context.UserAlbums.FirstOrDefaultAsync(ua =>
                ua.ApplicationUserId == user.Id && ua.AlbumId == albumId);

            if (userAlbum != null)
            {
                var sql = "UPDATE UserAlbums SET IsFavourite = 1 WHERE ApplicationUserId = @p0 AND AlbumId = @p1";
                await _context.Database.ExecuteSqlRawAsync(sql, user.Id, albumId);
            }
            else
            {
                var sql = "INSERT INTO UserAlbums (ApplicationUserId, AlbumId, IsFavourite,IsOnWishlist,IsListening) VALUES (@p0, @p1, 1, 0, 0)";
                await _context.Database.ExecuteSqlRawAsync(sql, user.Id, albumId);
            }

            return RedirectToAction("Index", "Albums");
        }




        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> RemoveFromFavourites(int albumId)
        {
            var user = await _userManager.GetUserAsync(User);
            var sql = "UPDATE UserAlbums SET IsFavourite = 0 WHERE ApplicationUserId = @p0 AND AlbumId = @p1";
            await _context.Database.ExecuteSqlRawAsync(sql, user.Id, albumId);
            return RedirectToAction("MyAlbums", "Account");
        }


        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyFavourites()
        {
            var user = await _userManager.GetUserAsync(User);
            var favouriteAlbums = _context.UserAlbums
                .Include(ua => ua.Album)
                .Where(ua => ua.ApplicationUserId == user.Id && ua.IsFavourite == true)
                .Select(ua => ua.Album)
                .ToList();

            return View(favouriteAlbums);
        }



        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddToWishlist(int albumId)
        {
            var user = await _userManager.GetUserAsync(User);
            var sql = @"
    IF EXISTS (SELECT 1 FROM UserAlbums WHERE ApplicationUserId = @p0 AND AlbumId = @p1)
        UPDATE UserAlbums SET IsOnWishlist = 1 WHERE ApplicationUserId = @p0 AND AlbumId = @p1
    ELSE
        INSERT INTO UserAlbums (ApplicationUserId, AlbumId, IsOnWishlist,IsListening, isFavourite) VALUES (@p0, @p1, 1,0, 0)";
            await _context.Database.ExecuteSqlRawAsync(sql, user.Id, albumId);
            return RedirectToAction("Index", "Albums");
        }


        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> RemoveFromWishlist(int albumId)
        {
            var user = await _userManager.GetUserAsync(User);
            var sql = "UPDATE UserAlbums SET IsOnWishlist = 0 WHERE ApplicationUserId = @p0 AND AlbumId = @p1";
            await _context.Database.ExecuteSqlRawAsync(sql, user.Id, albumId);
            return RedirectToAction("MyAlbums", "Account");
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddToListening(int albumId)
        {
            var user = await _userManager.GetUserAsync(User);
            var sql = @"
    IF EXISTS (SELECT 1 FROM UserAlbums WHERE ApplicationUserId = @p0 AND AlbumId = @p1)
        UPDATE UserAlbums SET IsListening = 1 WHERE ApplicationUserId = @p0 AND AlbumId = @p1
    ELSE
        INSERT INTO UserAlbums (ApplicationUserId, AlbumId, IsListening, isFavourite, IsOnWishlist) VALUES (@p0, @p1, 1, 0, 0)";
            await _context.Database.ExecuteSqlRawAsync(sql, user.Id, albumId);
            return RedirectToAction("Index", "Albums");
        }



        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> RemoveFromListening(int albumId)
        {
            var user = await _userManager.GetUserAsync(User);
            var sql = "UPDATE UserAlbums SET IsListening = 0 WHERE ApplicationUserId = @p0 AND AlbumId = @p1";
            await _context.Database.ExecuteSqlRawAsync(sql, user.Id, albumId);
            return RedirectToAction("MyAlbums", "Account");
        }





        public async Task<IActionResult> FriendsAlbums(string friendId)
        {
            var friend = await _userManager.FindByIdAsync(friendId);
            var friendName = friend.FullName; 

            var favouriteAlbums = await _context.UserAlbums
                .Where(x => x.ApplicationUserId == friendId && x.IsFavourite)
                .Select(x => x.Album)
                .ToListAsync();

            var wishlistAlbums = await _context.UserAlbums
                .Where(x => x.ApplicationUserId == friendId && x.IsOnWishlist)
                .Select(x => x.Album)
                .ToListAsync();

            var listeningAlbums = await _context.UserAlbums
                .Where(x => x.ApplicationUserId == friendId && x.IsListening)
                .Select(x => x.Album)
                .ToListAsync();

            var viewModel = new UserAlbumsVM
            {
                FriendName = friendName,
                FavouriteAlbums = favouriteAlbums,
                WishlistAlbums = wishlistAlbums,
                ListeningAlbums = listeningAlbums
            };

            return View(viewModel);
        }


        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyAlbums()
        {
            var user = await _userManager.GetUserAsync(User);
            var favouriteAlbums = await _context.UserAlbums
                .Where(x => x.ApplicationUserId == user.Id && x.IsFavourite)
                .Select(x => x.Album)
                .ToListAsync();

            var wishlistAlbums = await _context.UserAlbums
                .Where(x => x.ApplicationUserId == user.Id && x.IsOnWishlist)
                .Select(x => x.Album)
                .ToListAsync();

            var listeningAlbums = await _context.UserAlbums
                .Where(x => x.ApplicationUserId == user.Id && x.IsListening)
                .Select(x => x.Album)
                .ToListAsync();

            var viewModel = new UserAlbumsVM
            {
                FavouriteAlbums = favouriteAlbums,
                WishlistAlbums = wishlistAlbums,
                ListeningAlbums = listeningAlbums
            };

            return View(viewModel);
        }









    }
}
