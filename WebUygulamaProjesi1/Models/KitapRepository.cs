using WebUygulamaProjesi1.Utility;

namespace WebUygulamaProjesi1.Models
{
    public class KitapRepository : Repository<Kitap>, IKitapRepository
    {
        private UygulamaDbContext _uygulamaDbContext;

        public KitapRepository(UygulamaDbContext uygulamaDbContext) : base(uygulamaDbContext)
        {
            _uygulamaDbContext = uygulamaDbContext; // DI

            /**
             * Bu constructor base'deki constructor'u çağırır. Base constructor'da ilgili 
             * uygulamaDbContext oluşturulacak ve DI ile gelecek.
             */
        }

        public void Guncelle(Kitap kitap)
        {
            _uygulamaDbContext.Update(kitap);
        }

        public void Kaydet()
        {
            _uygulamaDbContext.SaveChanges();
        }
    }
}
