using System;

namespace KutuphaneYonetimSistemi
{
    // İşlem sınıfı - kitap işlemlerini tutar
    public class Islem
    {
        // İşlemin yapıldığı kitabın adı
        public string KitapAdi { get; set; }

        // İşlem tipi (Ödünç Alma/İade)
        public string Tip { get; set; }

        // İşlem tarihi
        public DateTime Tarih { get; set; }

        // İşlem bilgilerini string olarak döndürür
        public override string ToString()
        {
            return $"{KitapAdi} - {Tip} - {Tarih:dd.MM.yyyy HH:mm}";
        }
    }
}
