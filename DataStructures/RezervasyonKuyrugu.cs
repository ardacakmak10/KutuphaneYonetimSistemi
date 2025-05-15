using System.Collections.Generic;

namespace KutuphaneYonetimSistemi
{
    // Kitap rezervasyonlarını FIFO (ilk giren ilk çıkar) mantığıyla saklayan kuyruk sınıfı
    public class RezervasyonKuyrugu
    {
        // Rezervasyonları tutan kuyruk veri yapısı
        private Queue<Rezervasyon> kuyruk = new Queue<Rezervasyon>();

        // Verilen rezervasyonu kuyruğun sonuna ekler (yeni gelen en sona eklenir)
        public void Ekle(Rezervasyon rezervasyon)
        {
            kuyruk.Enqueue(rezervasyon);
        }

        // Kuyruktaki ilk rezervasyonu (en eski) gösterir ama kuyruktan çıkarmaz
        // Eğer kuyruk boşsa null değer döndürür
        public Rezervasyon Siradaki()
        {
            return kuyruk.Count > 0 ? kuyruk.Peek() : null;
        }

        // Kuyruktaki ilk rezervasyonu (en eski) çıkarır ve döndürür
        // Eğer kuyruk boşsa null değer döndürür
        public Rezervasyon Cikar()
        {
            return kuyruk.Count > 0 ? kuyruk.Dequeue() : null;
        }

        // Kuyruktaki tüm rezervasyonları sırasıyla (eskiden yeniye) liste olarak döndürür
        public List<Rezervasyon> TumunuListele()
        {
            return new List<Rezervasyon>(kuyruk);
        }

        // Kuyrukta hiç rezervasyon olup olmadığını kontrol eder
        // true: kuyruk boş, false: kuyrukta en az bir rezervasyon var
        public bool BosMu()
        {
            return kuyruk.Count == 0;
        }
    }
}
