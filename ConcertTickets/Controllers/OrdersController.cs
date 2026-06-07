using ConcertTickets.Data;
using ConcertTickets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConcertTickets.Controllers
{
    [Authorize]   // sipariş işlemleri için giriş şart
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // Bilet alma formunu GÖSTERİR (GET)
        public async Task<IActionResult> Buy(int? id)
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

        // Satın almayı İŞLER (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(int concertId, int quantity)
        {
            var concert = await _context.Concerts.FindAsync(concertId);
            if (concert == null)
            {
                return NotFound();
            }

            // 1) Adet geçerli mi?
            if (quantity < 1)
            {
                ModelState.AddModelError("", "En az 1 bilet almalısınız.");
                return View(concert);
            }

            // 2) Yeterli kontenjan var mı?
            int remaining = concert.Capacity - concert.Sold;
            if (quantity > remaining)
            {
                ModelState.AddModelError("", $"Yeterli bilet yok. Kalan: {remaining}");
                return View(concert);
            }

            // 3) Siparişi oluştur
            var order = new Order
            {
                ConcertId = concert.Id,
                UserId = _userManager.GetUserId(User),
                Quantity = quantity,
                TotalPrice = quantity * concert.Price,
                OrderDate = DateTime.Now
            };

            // 4) Satılan adedi arttır (stok düş)
            concert.Sold += quantity;

            // 5) İkisini birden kaydet
            _context.Orders.Add(order);
            _context.Concerts.Update(concert);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MyOrders));
        }

        // Kullanıcının kendi siparişleri
        public async Task<IActionResult> MyOrders()
        {
            var userId = _userManager.GetUserId(User);

            var orders = await _context.Orders
                .Include(o => o.Concert)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

    }
}