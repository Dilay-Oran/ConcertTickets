using ConcertTickets.Data;
using ConcertTickets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConcertTickets.Controllers
{
    [Authorize]
    public class ConcertsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ConcertsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var concerts = await _context.Concerts.ToListAsync();
            return View(concerts);
        }

        [Authorize(Roles = "Admin,Organizer")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<IActionResult> Create(Concert concert, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                // Resim yüklendiyse kaydet ve yolunu sakla
                var imagePath = await ImageService.SaveResizedImageAsync(imageFile, _env.WebRootPath);
                if (imagePath != null)
                {
                    concert.ImagePath = imagePath;
                }

                _context.Add(concert);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(concert);
        }

        [Authorize(Roles = "Admin,Organizer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concert = await _context.Concerts.FindAsync(id);
            if (concert == null)
            {
                return NotFound();
            }

            return View(concert);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<IActionResult> Edit(int id, Concert concert, IFormFile? imageFile)
        {
            if (id != concert.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Mevcut kaydı çek (eski resim yolunu öğrenmek için)
                var existing = await _context.Concerts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
                if (existing == null)
                {
                    return NotFound();
                }

                // Yeni resim yüklendiyse: eskiyi sil, yeniyi kaydet. Yoksa eskiyi koru.
                var newImagePath = await ImageService.SaveResizedImageAsync(imageFile, _env.WebRootPath);
                if (newImagePath != null)
                {
                    ImageService.DeleteImage(existing.ImagePath, _env.WebRootPath);
                    concert.ImagePath = newImagePath;
                }
                else
                {
                    concert.ImagePath = existing.ImagePath;
                }

                _context.Update(concert);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(concert);
        }

        [Authorize(Roles = "Admin,Organizer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concert = await _context.Concerts.FirstOrDefaultAsync(m => m.Id == id);
            if (concert == null)
            {
                return NotFound();
            }

            return View(concert);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var concert = await _context.Concerts.FindAsync(id);
            if (concert != null)
            {
                _context.Concerts.Remove(concert);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}   