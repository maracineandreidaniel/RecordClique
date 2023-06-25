using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecordClique.Data;
using RecordClique.Models;

namespace RecordClique.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly AppDbContext _context;

        public AlbumsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Albums
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Albums.Include(a => a.Artist).Include(a => a.Label);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Albums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Albums == null)
            {
                return NotFound();
            }

            var album = await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Label)                
                .FirstOrDefaultAsync(m => m.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // GET: Albums/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "ArtistName");
            ViewData["LabelID"] = new SelectList(_context.Labels, "Id", "LabelName");          
            return View();
        }

        // POST: Albums/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AlbumName,AlbumDescription,AlbumPrice,AlbumCoverURL,AlbumReleaseDate,AlbumGenre,LabelID,ArtistId")] Album album)
        {
            if (ModelState.IsValid)
            {
                _context.Add(album);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "ArtistName", album.ArtistId);
            ViewData["LabelID"] = new SelectList(_context.Labels, "Id", "LabelName", album.LabelID);            
            return View(album);
        }

        // GET: Albums/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Albums == null)
            {
                return NotFound();
            }

            var album = await _context.Albums.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "ArtistName", album.ArtistId);
            ViewData["LabelID"] = new SelectList(_context.Labels, "Id", "LabelName", album.LabelID);          
            return View(album);
        }

        // POST: Albums/Edit/5
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AlbumName,AlbumDescription,AlbumPrice,AlbumCoverURL,AlbumReleaseDate,AlbumGenre,LabelID,ArtistId")] Album album)
        {
            if (id != album.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(album);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(album.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "ArtistName", album.ArtistId);
            ViewData["LabelID"] = new SelectList(_context.Labels, "Id", "LabelName", album.LabelID);
            return View(album);
        }

        // GET: Albums/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Albums == null)
            {
                return NotFound();
            }

            var album = await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Label)                
                .FirstOrDefaultAsync(m => m.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Albums == null)
            {
                return Problem("Entity set 'AppDbContext.Albums'  is null.");
            }
            var album = await _context.Albums.FindAsync(id);
            if (album != null)
            {
                _context.Albums.Remove(album);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumExists(int id)
        {
          return _context.Albums.Any(e => e.Id == id);
        }


        public async Task<IActionResult> Filter(string albumName)
        {
            IQueryable<Album> appDbContext = _context.Albums.Include(a => a.Artist).Include(a => a.Label);

            if (!String.IsNullOrEmpty(albumName))
            {
                appDbContext = appDbContext.Where(a => a.AlbumName.Contains(albumName));
            }

            return View("Index", await appDbContext.ToListAsync());
        }


        public async Task<IActionResult> AlbumComments(int albumId)
        {
            var comments = await _context.Comments
                .Include(c => c.User)
                .Where(c => c.AlbumId == albumId)
                .ToListAsync();

            return View(comments);
        }


    }
}
