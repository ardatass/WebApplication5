using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext dbContext;

        public HomeController()
        {
            dbContext = new AppDbContext();
        }

        public ActionResult Index()
        {
            // Kullanıcı adını almak için Session'dan veri çek
            ViewBag.Username = Session["Username"];

            // Veritabanından müşteri listesini çek
            var musteriler = dbContext.MusteriTablosu.ToList();

            // Müşteri listesini Index görünümüne model olarak gönder
            return View(musteriler);
        }

        [HttpPost]
        public ActionResult MusteriyiKaydet(Musteri musteri)
        {
            if (ModelState.IsValid)
            {
                // Yeni müşteri kaydet
                dbContext.MusteriTablosu.Add(musteri);
                dbContext.SaveChanges();

                // Güncellenmiş müşteri listesini al
                var musteriler = dbContext.MusteriTablosu.ToList();
                return View("About", musteriler);
            }
            return View("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var musteri = dbContext.MusteriTablosu.Find(id);
            if (musteri == null)
            {
                return HttpNotFound();
            }
            return View(musteri);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Musteri model)
        {
            if (ModelState.IsValid)
            {
                var existingMusteri = await dbContext.MusteriTablosu
                    .FirstOrDefaultAsync(m => m.MusteriID == model.MusteriID);

                if (existingMusteri != null)
                {
                    existingMusteri.AdSoyad = model.AdSoyad;
                    existingMusteri.Emaıl = model.Emaıl;
                    existingMusteri.Telefon = model.Telefon;

                    await dbContext.SaveChangesAsync();
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Müşteri bulunamadı." });
            }
            return Json(new { success = false, message = "Geçersiz veri." });
        }

        public ActionResult About()
        {
          
            ViewBag.Username = Session["Username"];

            // Müşteri listesini About görünümüne model olarak gönder
            var musteriler = dbContext.MusteriTablosu.ToList();
            return View(musteriler);
        }

        public ActionResult Contact()
        {
       
            ViewBag.Username = Session["Username"];

            var aktiviteler = dbContext.aktivites.ToList();
            return View(aktiviteler);
        }

        // GET: ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: ChangePassword
        [HttpPost]
        public ActionResult ChangePassword(ChangePassword changePassword)
        {
            if (ModelState.IsValid)
            {
             
                string currentPasswordHash = ComputeSha256Hash(changePassword.CurrentPassword);

                
                var user = dbContext.Users
                    .FirstOrDefault(p => p.Password == currentPasswordHash);

                if (user != null)
                {
                    // Yeni parolayı hashle
                    user.Password = ComputeSha256Hash(changePassword.NewPassword);
                    dbContext.SaveChanges(); // Değişiklikleri veritabanına kaydet

                    TempData["SuccessMessage"] = "Parolanız başarıyla güncellendi.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Mevcut parola doğru değil.");
                }
            }

            return View(changePassword);
        }

       
        private static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
               
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public ActionResult Profil()
        {
            return View();
        }

    }
}