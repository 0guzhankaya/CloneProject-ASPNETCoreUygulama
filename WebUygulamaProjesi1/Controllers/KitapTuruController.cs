using Microsoft.AspNetCore.Mvc;
using WebUygulamaProjesi1.Models;
using WebUygulamaProjesi1.Utility;

namespace WebUygulamaProjesi1.Controllers
{
    public class KitapTuruController : Controller
    {
        private readonly IKitapTuruRepository _kitapTuruRepository; //Dependency Injection, Service Container'dan geldi.

        //Constructor
        public KitapTuruController(IKitapTuruRepository context)
        {
            _kitapTuruRepository = context;
        }

        public IActionResult Index()
        {
            List<KitapTuru> objKitapTuruList = _kitapTuruRepository.GetAll().ToList();
            //Veritabanından Kitap Türlerini Listeye Çeker.
            return View(objKitapTuruList); //Controller, View'a iletir.
        }

        public IActionResult Ekle()
        {
            return View();
        }

        //Front-End'den post edilen veriyi EF ile Database'e insert eden method.
        [HttpPost]
        public IActionResult Ekle(KitapTuru kitapTuru)
        {
            /**
             * EF'le DB'ye kayıt eklemek için.
             * Kitap ekleme görevi EF'e gelir. Buna ekle demek için kullanılır.
             * SaveChanges() methodu çağırıldığında ekleme işlemi yapılmış olur.
             * SaveChanges() methodu ile eklenecek verileri biriktirip bir kerede veritabanına ekleme yapar.
             * DB'ye git gel maliyetinin önüne geçen bir sistemdir.
             * SaveChanges() methodu çağırılmazsa veriler db'ye kayıt edilmez.
             * Kullanıcı null veya 25 karakterden uzun veri girişi gerçekleştirdiğinde uygulamanın crash vermemesi için,
             * check yapıp sonrasında SaveChanges() ile DB'ye kaydetmemiz gerekiyor.
             */

            //Sunucuda tespit edilen validation hatası
            if (ModelState.IsValid) //Model class'da tanımlanan şartların sağlanmasını kontrol eder.
            {
                _kitapTuruRepository.Ekle(kitapTuru);
                _kitapTuruRepository.Kaydet();
                TempData["basarili"] = "Yeni Kitap Türü Başarıyla Oluşturuldu.";
                return RedirectToAction("Index", "KitapTuru");
            }
            return View(); //Eğer valid değilse kullanıcı, ekleme sayfasına geri yönlendirilir.

            /**
             * DB'ye yeni kayıt eklendikten sonra web sayfası Index Action'una gidecek.
             * Aynı Controller içerisinde bulunduğu için Controller ismi yazılmayabilir ancak
             * normalde yazılması gerekiyor.
             */
        }

        public IActionResult Guncelle(int? id) // ? nullable anlamına gelmektedir.
        {
            if (id == null || id == 0)
            {
                return NotFound(); //ASP'nin Not Found ekranını otomatik oluşturan fonksiyonu
            }
            /**
             * null check'den başarılı bir şekilde geçildikten sonra kitap türünü veritabanından çekmek gerekiyor.
             * DI nesnesi kullanılarak EF'den gelen Find() methodu ile otomatik bir şekilde kullanıcının verdiği id'li 
             * nesneyi veritabanından bulup getirir ve nesneye çevirir.
             * */
            KitapTuru? kitapTuruVt = _kitapTuruRepository.Get(u => u.Id == id); //Expression<Func<T, bool>> filtre
            //Guncelle Action'a gönderilen id'ye eşit olan kaydı getirir.
            //Null gelebileceği için warning verir. Nullable işaretlenirse warning vermez.

            if (kitapTuruVt == null) 
            { 
                return NotFound(); 
            }

            return View(kitapTuruVt); //view'a gidecek nesne. Views.Guncelle.cshtml'i burası çağırır.
        }

        [HttpPost]
        public IActionResult Guncelle(KitapTuru kitapTuru)
        {
            if (ModelState.IsValid) 
            {
                _kitapTuruRepository.Guncelle(kitapTuru);
                _kitapTuruRepository.Kaydet();
                TempData["basarili"] = "Kitap Türü Başarıyla Güncellendi.";
                return RedirectToAction("Index", "KitapTuru");
            }
            return View();
        }

        //GET ACTION : Sil.cshtml dosyasını GETirir.
        public IActionResult Sil(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            KitapTuru? kitapTuruVt = _kitapTuruRepository.Get(u => u.Id == id);

            if (kitapTuruVt == null) 
            { 
                return NotFound(); 
            }
            return View(kitapTuruVt);
        }

        
         //POST ACTION  
         //ActionName Attribute ile isim verildi.
         
        [HttpPost, ActionName("Sil")]
        public IActionResult SilPOST(int? id)
        {
            if (ModelState.IsValid)
            {
                KitapTuru? kitapTuru = _kitapTuruRepository.Get(u => u.Id == id);
                if (kitapTuru == null)
                {
                    return NotFound();
                }
                _kitapTuruRepository.Sil(kitapTuru);
                _kitapTuruRepository.Kaydet();
                TempData["basarili"] = "Silme İşlemi Başarılı";
                return RedirectToAction("Index", "KitapTuru");
            }
            return View();
        }
    }
}
