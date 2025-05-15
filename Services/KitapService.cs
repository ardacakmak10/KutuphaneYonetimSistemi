using System;
using System.Collections.Generic;

namespace KutuphaneYonetimSistemi
{
    // Kütüphane sistemindeki kitap işlemlerini yöneten servis sınıfı
    public class KitapService
    {
        private static KitapService _instance;
        private static readonly object _lock = new object();
        private BagliListe _kitapListesi;
        private KitapAgaci _kitapAgaci;

        // Özel yapıcı metod - Singleton tasarım deseni için
        private KitapService()
        {
            _kitapListesi = new BagliListe();
            _kitapAgaci = new KitapAgaci();
        }

        // Servisin tek örneğini döndüren özellik - Singleton tasarım deseni
        public static KitapService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new KitapService();
                        }
                    }
                }
                return _instance;
            }
        }

        // Yeni bir kitabı bağlı listenin başına ve ağaca ekler
        public void BasaEkle(Kitap kitap)
        {
            _kitapListesi.BasaEkle(kitap);
            _kitapAgaci.Ekle(kitap);
        }

        // Yeni bir kitabı bağlı listenin sonuna ve ağaca ekler
        public void SonaEkle(Kitap kitap)
        {
            _kitapListesi.SonaEkle(kitap);
            _kitapAgaci.Ekle(kitap);
        }

        // Yeni bir kitabı verilen iki ISBN numarası arasına ve ağaca ekler
        public void ArayaEkle(Kitap kitap, string isbn1, string isbn2)
        {
            _kitapListesi.ArayaEkle(kitap, isbn1, isbn2);
            _kitapAgaci.Ekle(kitap);
        }

        // Verilen ISBN numarasına sahip kitabı bağlı listeden siler
        public void Sil(string isbn)
        {
            _kitapListesi.Sil(isbn);
        }

        // Bağlı listedeki tüm kitapları döndürür
        public IEnumerable<Kitap> TumKitaplariGetir()
        {
            return _kitapListesi.Listele();
        }

        // Ağaçtaki tüm kitapları sıralı olarak döndürür
        public IEnumerable<Kitap> AgactanTumKitaplariGetir()
        {
            return _kitapAgaci.InorderListele();
        }

        // Verilen ISBN numarasına sahip kitabı ağaçta arar ve bulursa döndürür
        public Kitap AgactanKitapAra(string isbn)
        {
            return _kitapAgaci.Ara(isbn);
        }

        // Bu metot, girilen kriterlere göre kitap listesinden filtrelenmiş kitapları döner.
        // Tüm parametreler isteğe bağlıdır, boş bırakılan kriterler göz ardı edilir.
        public IEnumerable<Kitap> KitaplariFiltrele(string ad = "", string isbn = "", string yazar = "", string yayinYili = "")
        {
            foreach (var kitap in _kitapListesi.Listele())
            {
                bool eslesiyor = true;

                // Kitap adı filtrelemesi
                if (!string.IsNullOrWhiteSpace(ad) && !kitap.Ad.ToLower().Contains(ad.ToLower()))
                    eslesiyor = false;

                // ISBN filtrelemesi
                if (!string.IsNullOrWhiteSpace(isbn) && !kitap.ISBN.ToLower().Contains(isbn.ToLower()))
                    eslesiyor = false;

                // Yazar filtrelemesi
                if (!string.IsNullOrWhiteSpace(yazar) && !kitap.Yazar.ToLower().Contains(yazar.ToLower()))
                    eslesiyor = false;

                // Yayın yılı filtrelemesi
                if (!string.IsNullOrWhiteSpace(yayinYili) && kitap.YayinYili.ToString() != yayinYili)
                    eslesiyor = false;

                // Tüm kriterlere uyan kitaplar döndürülür
                if (eslesiyor)
                    yield return kitap;
            }
        }

        // Bu metot, ikili arama ağacındaki kitaplar içinde ISBN parçası geçenleri liste olarak döner.
        // ISBN'in tamamı değil, içinde geçen herhangi bir kısım eşleşebilir.
        public List<Kitap> ISBNIcerenleriGetir(string isbnParcasi)
        {
            return _kitapAgaci.ISBNIcerenleriAra(isbnParcasi);
        }
    }
} 