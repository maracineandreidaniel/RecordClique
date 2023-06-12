using RecordClique.Data;
using RecordClique.Data;
using RecordClique.Data.Static;
//using RecordClique.Data.Static;
using RecordClique.Data.ViewModels;
using RecordClique.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var users = await _context.Users.ToListAsync();
            return View(users);
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

          //  if (newUserResponse.Succeeded)
            //    await _userManager.AddToRoleAsync(newUser, "Admin");

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
        public async Task<IActionResult> AddToFavourite(int albumId)
        {
            var user = await _userManager.GetUserAsync(User);
            var userAlbum = _context.UserAlbums.FirstOrDefault(ua =>
                ua.ApplicationUserId == user.Id && ua.AlbumId == albumId);

            if (userAlbum != null)
            {
                userAlbum.IsFavourite = true;
            }
            else
            {
                userAlbum = new UserAlbum
                {
                    ApplicationUserId = user.Id,
                    AlbumId = albumId,
                    IsFavourite = true
                };
                _context.UserAlbums.Add(userAlbum);
            }

            await _context.SaveChangesAsync();
            // Redirect to the appropriate view, e.g., details view for the album
            return RedirectToAction("Index", "Albums");
        }


        [HttpPost]
        public async Task<IActionResult> RemoveFromFavourite(int albumId)
       
        {
            var user = await _userManager.GetUserAsync(User);
            var userAlbum = _context.UserAlbums.FirstOrDefault(ua =>
                ua.ApplicationUserId == user.Id && ua.AlbumId == albumId);
              

            if (userAlbum != null)
            {
                userAlbum.IsFavourite = false;
                await _context.SaveChangesAsync();
            }

            // Redirect to the appropriate view, e.g., details view for the album
            return RedirectToAction("Index", "Albums");
        }


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
        public async Task<IActionResult> AddToWishlist(int albumId)
        {
            var user = await _userManager.GetUserAsync(User);
            var userAlbum = _context.UserAlbums.FirstOrDefault(ua =>
                ua.ApplicationUserId == user.Id && ua.AlbumId == albumId);

            if (userAlbum != null)
            {
                userAlbum.IsOnWishlist = true;
            }
            else
            {
                userAlbum = new UserAlbum
                {
                    ApplicationUserId = user.Id,
                    AlbumId = albumId,
                    IsOnWishlist = true
                };
                _context.UserAlbums.Add(userAlbum);
            }

            await _context.SaveChangesAsync();
            // Redirect to the appropriate view, e.g., details view for the album
            return RedirectToAction("Index", "Albums");
        }


        [HttpPost]
        public async Task<IActionResult> RemoveFromWishlist(int albumId)

        {
            var user = await _userManager.GetUserAsync(User);
            var userAlbum = _context.UserAlbums.FirstOrDefault(ua =>
                ua.ApplicationUserId == user.Id && ua.AlbumId == albumId);


            if (userAlbum != null)
            {
                userAlbum.IsOnWishlist = false;
                await _context.SaveChangesAsync();
            }

            // Redirect to the appropriate view, e.g., details view for the album
            return RedirectToAction("Index", "Albums");
        }


        [HttpPost]
        public async Task<IActionResult> AddToListening(int albumId)
        {
            var user = await _userManager.GetUserAsync(User);
            var userAlbum = _context.UserAlbums.FirstOrDefault(ua =>
                ua.ApplicationUserId == user.Id && ua.AlbumId == albumId);

            if (userAlbum != null)
            {
                userAlbum.IsListening = true;
            }
            else
            {
                userAlbum = new UserAlbum
                {
                    ApplicationUserId = user.Id,
                    AlbumId = albumId,
                    IsListening = true
                };
                _context.UserAlbums.Add(userAlbum);
            }

            await _context.SaveChangesAsync();
            // Redirect to the appropriate view, e.g., details view for the album
            return RedirectToAction("Index", "Albums");
        }


        [HttpPost]
        public async Task<IActionResult> RemoveFromListening(int albumId)

        {
            var user = await _userManager.GetUserAsync(User);
            var userAlbum = _context.UserAlbums.FirstOrDefault(ua =>
                ua.ApplicationUserId == user.Id && ua.AlbumId == albumId);


            if (userAlbum != null)
            {
                userAlbum.IsListening = false;
                await _context.SaveChangesAsync();
            }

            // Redirect to the appropriate view, e.g., details view for the album
            return RedirectToAction("Index", "Albums");
        }





        public async Task<IActionResult> FriendsAlbums(string friendId)
        {
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
                FavouriteAlbums = favouriteAlbums,
                WishlistAlbums = wishlistAlbums,
                ListeningAlbums = listeningAlbums
            };

            return View(viewModel);
        }


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
