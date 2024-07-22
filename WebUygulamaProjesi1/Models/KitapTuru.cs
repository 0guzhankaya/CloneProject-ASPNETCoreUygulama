using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebUygulamaProjesi1.Models
{
    public class KitapTuru
    {
        [Key] //Primary Key
        public int Id { get; set; }

        [Required(ErrorMessage = "Kitap Tür Adı Boş Bırakılamaz!")] //Not Null, null kaldığında verilecek hata mesajı.
        [MaxLength(25)] //Kullanıcıdan alınacak veriyi 25 karakter ile sınırlar.
        [DisplayName("Kitap Türü Adı")] //asp-for=Ad kullanıldığı zaman frontend'de gözükecek.
        public string Ad { get; set; }
    }
}
