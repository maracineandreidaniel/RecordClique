using RecordClique.Data;
using RecordClique.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecordClique.Data.ViewModels;

public class FriendController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _context;

    public FriendController(UserManager<ApplicationUser> userManager, AppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> AddFriend(string friendId)
    {
        var user = await _userManager.GetUserAsync(User);
        var friend = await _userManager.FindByIdAsync(friendId);

        if (friend == null)
        {
            return NotFound("Friend not found");
        }

        var friendship = new Friendship
        {
            InitiatorId = user.Id,
            FriendId = friend.Id
        };

        _context.Friendships.Add(friendship);
        await _context.SaveChangesAsync();

        return RedirectToAction("Users", "Account");

    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var friends = _context.Friendships
            .Where(f => f.InitiatorId == user.Id || f.FriendId == user.Id)
            .Select(f => f.InitiatorId == user.Id ? f.Friend : f.Initiator)
            .ToList();
        return View(friends);
    }


    [HttpPost]
    public async Task<IActionResult> RemoveFriend(string friendId)
    {
        var user = await _userManager.GetUserAsync(User);

        var friendship = _context.Friendships.FirstOrDefault(f =>
            (f.InitiatorId == user.Id && f.FriendId == friendId) ||
            (f.FriendId == user.Id && f.InitiatorId == friendId));

        if (friendship != null)
        {
            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("MyFriends", "Account");
    }

    public async Task<IActionResult> MyFriends()
    {
        var user = await _userManager.GetUserAsync(User);
        var friends = _context.Friendships
            .Where(f => f.InitiatorId == user.Id || f.FriendId == user.Id)
            .Select(f => f.InitiatorId == user.Id ? f.Friend : f.Initiator)
            .ToList();
        return View(friends);
    }

    public async Task<IActionResult> ViewFriendFavourites(string friendId)
    {
        var favouriteAlbums = _context.UserAlbums
            .Include(ua => ua.Album)
            .Where(ua => ua.ApplicationUserId == friendId && ua.IsFavourite == true)
            .Select(ua => ua.Album)
            .ToList();

        return View(favouriteAlbums);
    }


    public async Task<IActionResult> ViewFriendWishlist(string friendId)
    {
        var wishlistAlbums = _context.UserAlbums
            .Include(ua => ua.Album)
            .Where(ua => ua.ApplicationUserId == friendId && ua.IsOnWishlist == true)
            .Select(ua => ua.Album)
            .ToList();

        return View(wishlistAlbums);
    }


    public async Task<IActionResult> ViewFriendListening(string friendId)
    {
        var wishlistAlbums = _context.UserAlbums
            .Include(ua => ua.Album)
            .Where(ua => ua.ApplicationUserId == friendId && ua.IsListening == true)
            .Select(ua => ua.Album)
            .ToList();

        return View(wishlistAlbums);
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






}
