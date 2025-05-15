using System;

namespace KutuphaneYonetimSistemi
{
    // Kütüphane sistemindeki kitap rezervasyonlarını temsil eden sınıf
    public class Rezervasyon
    {
        // Rezervasyonu yapan kullanıcının adı
        public string KullaniciAdi { get; set; }

        // Rezerve edilen kitabın ISBN numarası
        public string KitapISBN { get; set; }

        // Rezerve edilen kitabın adı
        public string KitapAdi { get; set; }

        // Rezervasyonun yapıldığı tarih ve saat
        public DateTime RezervasyonTarihi { get; set; }

        // Rezervasyon bilgilerini "KullaniciAdi → KitapAdi (ISBN) - Tarih" formatında bir metne dönüştürür
        public override string ToString()
        {
            return $"{KullaniciAdi} → {KitapAdi} (ISBN: {KitapISBN}) - {RezervasyonTarihi:dd.MM.yyyy HH:mm}";
        }
    }
}

