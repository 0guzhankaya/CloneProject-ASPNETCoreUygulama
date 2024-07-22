using WebUygulamaProjesi1.Utility;

namespace WebUygulamaProjesi1.Models
{
    public class KitapTuruRepository : Repository<KitapTuru>, IKitapTuruRepository
    {
        private UygulamaDbContext _uygulamaDbContext;

        public KitapTuruRepository(UygulamaDbContext uygulamaDbContext) : base(uygulamaDbContext)
        {
            _uygulamaDbContext = uygulamaDbContext; // DI

            /**
             * Bu constructor base'deki constructor'u çağırır. Base constructor'da ilgili 
             * uygulamaDbContext oluşturulacak ve DI ile gelecek.
             */
        }

        public void Guncelle(KitapTuru kitapTuru)
        {
            _uygulamaDbContext.Update(kitapTuru);
        }

        public void Kaydet()
        {
            _uygulamaDbContext.SaveChanges();
        }
    }
}
