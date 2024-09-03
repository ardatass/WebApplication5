using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApplication5.Migrations;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class AktiviteController : Controller
    {
        private readonly AppDbContext _context;

        public AktiviteController()
        {
            _context = new AppDbContext();
        }

        // GET: Aktivite
        public async Task<ActionResult> Contact()
        {
            var aktiviteler = await _context.aktivites.Include(a => a.Musteri).ToListAsync();
            return View(aktiviteler);
        }

        // GET: Aktivite/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var aktivite = await _context.aktivites.Include(a => a.Musteri).FirstOrDefaultAsync(a => a.AktiviteID == id);
            if (aktivite == null)
            {
                return HttpNotFound();
            }
            return View(aktivite);
        }

        // GET: Aktivite/Create
        public ActionResult Create()
        {
            ViewBag.MusteriID = new SelectList(_context.MusteriTablosu, "MusteriID", "AdSoyad");
            return View();
        }

        // POST: Aktivite/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Models.aktivite aktivite)
        {
            if (ModelState.IsValid)
            {
                // Tarih ve saati otomatik olarak al
                aktivite.AktiviteTarihi = DateTime.Now;

                // MusteriID'nin geçerli olup olmadığını kontrol et
                var musteriExists = await _context.MusteriTablosu.AnyAsync(m => m.MusteriID == aktivite.MusteriID);
                if (!musteriExists)
                {
                    ModelState.AddModelError("", "Geçersiz MusteriID");
                    ViewBag.MusteriID = new SelectList(_context.MusteriTablosu, "MusteriID", "AdSoyad", aktivite.MusteriID);
                    return View(aktivite);
                }

                _context.aktivites.Add(aktivite);
                await _context.SaveChangesAsync();

                // Aktivite eklendikten sonra HomeController'daki Contact sayfasına yönlendir
                return RedirectToAction("Contact", "Home");
            }

            ViewBag.MusteriID = new SelectList(_context.MusteriTablosu, "MusteriID", "AdSoyad", aktivite.MusteriID);
            return View(aktivite);
        }

        // POST: Aktivite/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Models.aktivite aktivite)
        {
            if (ModelState.IsValid)
            {
                var existingActivity = await _context.aktivites.FindAsync(aktivite.AktiviteID);

                if (existingActivity == null)
                {
                    return HttpNotFound();
                }

                // Mevcut tarih alanını koru
                existingActivity.AktiviteTipi = aktivite.AktiviteTipi;
                existingActivity.Aciklama = aktivite.Aciklama;
                existingActivity.MusteriID = aktivite.MusteriID;

                // Tarih alanını değiştirme
                // existingActivity.AktiviteTarihi = existingActivity.AktiviteTarihi; // Bu satırı yorum satırı olarak bırakın veya kaldırın

                // MusteriID'nin geçerli olup olmadığını kontrol et
                var musteriExists = await _context.MusteriTablosu.AnyAsync(m => m.MusteriID == aktivite.MusteriID);
                if (!musteriExists)
                {
                    ModelState.AddModelError("", "Geçersiz MusteriID");
                    ViewBag.MusteriID = new SelectList(_context.MusteriTablosu, "MusteriID", "AdSoyad", aktivite.MusteriID);
                    return View(aktivite);
                }

                try
                {
                    _context.Entry(existingActivity).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Contact", "Home");
                }
                catch (DbUpdateException ex)
                {
                    // Hata ayrıntılarını görmek için
                    Console.WriteLine(ex.Message);

                    // Kullanıcıya hata mesajını göster
                    ModelState.AddModelError("", "Bir hata oluştu. Lütfen tekrar deneyin.");
                }
            }

            ViewBag.MusteriID = new SelectList(_context.MusteriTablosu, "MusteriID", "AdSoyad", aktivite.MusteriID);
            return View(aktivite);
        }


        // GET: Aktivite/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var aktivite = await _context.aktivites.Include(a => a.Musteri).FirstOrDefaultAsync(a => a.AktiviteID == id);
            if (aktivite == null)
            {
                return HttpNotFound();
            }
            return View(aktivite);
        }

        // POST: Aktivite/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var aktivite = await _context.aktivites.FindAsync(id);
            _context.aktivites.Remove(aktivite);
            await _context.SaveChangesAsync();
            return RedirectToAction("Contact", "Home");
        }

        // Müşteri ID'sine göre aktiviteleri JSON formatında döndüren GET aksiyonu
        [HttpGet]
        public JsonResult GetActivities(int musteriID)
        {

            var activities = _context.aktivites
                .Where(a => a.MusteriID == musteriID)
                .Select(a => new
                {
                    a.AktiviteID,
                    a.AktiviteTipi,
                    a.Aciklama,
                    a.AktiviteTarihi,
                    
                })
                .ToList();

            return Json(activities, JsonRequestBehavior.AllowGet);
        }
    }
}

