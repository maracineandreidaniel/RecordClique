﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecordClique.Data;
using Microsoft.AspNetCore.Identity;
using RecordClique.Models;

namespace RecordClique.Controllers
{
    public class NewsFeedController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public NewsFeedController(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var friends = await _context.Friendships
                .Where(f => f.InitiatorId == userId)
                .Select(f => f.InitiatorId == userId ? f.FriendId : f.InitiatorId)
                .ToListAsync();

            friends.Add(userId);

            var logs = await _context.NewsFeedLogs
                .Where(log => friends.Contains(log.UserName))
                .OrderByDescending(log => log.Id) 
                .ToListAsync();

            ViewData["Context"] = _context;

            return View(logs);
        }




    }
}
