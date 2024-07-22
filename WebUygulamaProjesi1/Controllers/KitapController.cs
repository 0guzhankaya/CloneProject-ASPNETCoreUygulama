using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebUygulamaProjesi1.Models;
using WebUygulamaProjesi1.Utility;


namespace WebUygulamaProjesi1.Controllers
{
    public class KitapController : Controller
    {
        private readonly IKitapRepository _kitapRepository;
        private readonly IKitapTuruRepository _kitapTuruRepository;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public KitapController(IKitapRepository kitapRepository, IKitapTuruRepository kitapTuruRepository, IWebHostEnvironment webHostEnvironment)
        {
            _kitapRepository = kitapRepository;
            _kitapTuruRepository = kitapTuruRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Kitap> objKitapList = _kitapRepository.GetAll(includeProps:"KitapTuru").ToList();
            //Veritabanından kitap türlerini listeye çeker.
            return View(objKitapList); //Controller, View'a iletir.
        }

        public IActionResult EkleGuncelle(int? id)
        {
            IEnumerable<SelectListItem> kitapTuruList = _kitapTuruRepository.GetAll().Select(k => new SelectListItem
            {
                Text = k.Ad,
                Value = k.Id.ToString()
            });

            ViewBag.KitapTuruList = kitapTuruList; //Controller'dan View'a veri aktarır.

            if (id == null || id == 0)
            {
                //Ekle
                return View();
            }
            else
            {
                //Guncelle
                Kitap? kitapVt = _kitapRepository.Get(u => u.Id == id);

                if (kitapVt == null)
                    return NotFound();

                return View(kitapVt);
            }

        }

        //FrontEnd'den post edilen veriyi EF ile Database'e insert eden method.
        [HttpPost]
        public IActionResult EkleGuncelle(Kitap kitap, IFormFile? file)
        {
            //var errors = ModelState.Values.SelectMany(x => x.Errors);   

            //Server'da tespit edilen validation
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string kitapPath = Path.Combine(wwwRootPath, @"img");

                if (file != null)
                {
                    using (var fileStream = new FileStream(Path.Combine(kitapPath, file.FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    kitap.ResimUrl = @"/img/" + file.FileName;
                }

                //kitap.Id == 0 ? _kitapRepository.Ekle(kitap) : _kitapRepository.Guncelle(kitap);
                if (kitap.Id == 0)
                {
                    _kitapRepository.Ekle(kitap);
                    TempData["basarili"] = "Yeni Kitap Başarıyla Eklendi.";
                }
                else
                {
                    _kitapRepository.Guncelle(kitap);
                    TempData["basarili"] = "Kitap Başarıyla Güncellendi.";
                }

                _kitapRepository.Kaydet();
                return RedirectToAction("Index", "Kitap");
            }
            return View(); //Eğer valid değilse kullanıcı, ekleme sayfasına geri yönlendirilir.
        }


        /*
        public IActionResult Guncelle(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            Kitap? kitapVt = _kitapRepository.Get(u => u.Id == id);

            if (kitapVt == null)
                return NotFound();

            return View(kitapVt); //View'a gidecek nesne. Views.Guncelle.cshtml'i burası çağırır.
        }
        

        [HttpPost]
        public IActionResult Guncelle(Kitap kitap)
        {
            if (ModelState.IsValid)
            {
                _kitapRepository.Guncelle(kitap);
                _kitapRepository.Kaydet();
                TempData["basarili"] = "Kitap Başarıyla Güncellendi.";
                return RedirectToAction("Index", "Kitap");
            }
            return View();
        }
        */

        // GET ACTION : Sil.cshtml dosyasını GETirir.
        public IActionResult Sil(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            Kitap? kitapVt = _kitapRepository.Get(u => u.Id == id);

            if (kitapVt == null)
                return NotFound();

            return View(kitapVt);
        }

        [HttpPost, ActionName("Sil")]
        public IActionResult SilPOST(int? id)
        {
            if (ModelState.IsValid)
            {
                Kitap? kitap = _kitapRepository.Get(u => u.Id == id);

                if (kitap == null)
                    return NotFound();

                _kitapRepository.Sil(kitap);
                _kitapRepository.Kaydet();
                TempData["basarili"] = "Silme İşlemi Başarılı";
                return RedirectToAction("Index", "Kitap");
            }
            return View(); //Validasyon geçerli değilse kayıt ekranına yönlendirir.
        }
    }
}
