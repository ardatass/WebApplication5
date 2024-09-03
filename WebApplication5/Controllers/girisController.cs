using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class GirisController : Controller
    {
        private readonly AppDbContext dbContext;

        public GirisController()
        {
            dbContext = new AppDbContext();
        }

        // GET: Giriş yapma sayfası
        [HttpGet]
        public ActionResult Giris()
        {
            return View();
        }

        // POST: Giriş yapma işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Giris(User model)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcı doğrulama
                string hashedPassword = ComputeSha256Hash(model.Password);
                var user = dbContext.Users
                    .FirstOrDefault(u => u.Username == model.Username && u.Password == hashedPassword);

                if (user != null)
                {
                    // Başarılı giriş
                    Session["Username"] = user.Username;
                    Session["Password"] = user.Password; // Dikkat: Şifreyi sessionda tutmak güvenli değildir.

                    // Home/Index sayfasına yönlendirme
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Hatalı giriş
                    ModelState.AddModelError("", "Geçersiz kullanıcı adı veya parola.");
                }
            }
            else
            {
                // Model state geçersiz
                ModelState.AddModelError("", "Giriş bilgilerini kontrol ediniz.");
            }

            return View(model);
        }

        // GET: Kayıt olma sayfası
        [HttpGet]
        public ActionResult Kayit()
        {
            return View();
        }

        // POST: Kayıt olma işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kayit(User model)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcının veritabanında var olup olmadığını kontrol et
                var existingUser = dbContext.Users.FirstOrDefault(u => u.Username == model.Username);
                if (existingUser == null)
                {
                    try
                    {
                        // Şifreyi hashle
                        model.Password = ComputeSha256Hash(model.Password);

                        // Kayıt tarihini ekle
                        model.RegistrationDate = DateTime.Now;

                        // Kullanıcıyı veritabanına ekle
                        dbContext.Users.Add(model);
                        dbContext.SaveChanges();

                        // Başarılı kayıt sonrası giriş sayfasına yönlendirme
                        return RedirectToAction("Giris");
                    }
                    catch (Exception ex)
                    {
                        // Hata mesajını ekrana yazdır
                        ModelState.AddModelError("", $"Kayıt sırasında bir hata oluştu: {ex.Message}");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Bu kullanıcı adı zaten alınmış.");
                }
            }
            else
            {
                // Model state geçersiz
                ModelState.AddModelError("", "Kayıt bilgilerini kontrol ediniz.");
            }

            return View(model);
        }

        // SHA256 Hash hesaplama metodu
        private static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Girdi verisini hashle
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Hashi string formatına dönüştür
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
