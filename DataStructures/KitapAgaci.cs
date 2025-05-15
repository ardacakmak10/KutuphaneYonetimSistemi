using System.Collections.Generic;

namespace KutuphaneYonetimSistemi
{
    // İkili arama ağacı (BST) veri yapısı - Kitapları ISBN'e göre saklar ve yönetir
    public class KitapAgaci
    {
        // Ağacın kök düğümü
        public BstKitapNode Kok;

        // Kitap ekleme - ISBN'e göre ağaca yerleştirir
        public void Ekle(Kitap kitap)
        {
            Kok = EkleRecursive(Kok, kitap);
        }

        // Özyinelemeli kitap ekleme
        private BstKitapNode EkleRecursive(BstKitapNode dugum, Kitap kitap)
        {
            if (dugum == null)
                return new BstKitapNode(kitap);

            int karsilastir = string.Compare(kitap.ISBN, dugum.Veri.ISBN);
            if (karsilastir < 0)
                dugum.Sol = EkleRecursive(dugum.Sol, kitap);
            else if (karsilastir > 0)
                dugum.Sag = EkleRecursive(dugum.Sag, kitap);

            return dugum;
        }

        // ISBN'e göre kitap arama
        public Kitap Ara(string isbn)
        {
            return AraRecursive(Kok, isbn);
        }

        // Özyinelemeli kitap arama
        private Kitap AraRecursive(BstKitapNode dugum, string isbn)
        {
            if (dugum == null)
                return null;

            int karsilastir = string.Compare(isbn, dugum.Veri.ISBN);
            if (karsilastir == 0)
                return dugum.Veri;
            else if (karsilastir < 0)
                return AraRecursive(dugum.Sol, isbn);
            else
                return AraRecursive(dugum.Sag, isbn);
        }

        // ISBN parçasına göre kitap arama
        public List<Kitap> ISBNIcerenleriAra(string isbnParcasi)
        {
            List<Kitap> bulunanlar = new List<Kitap>();
            if (!string.IsNullOrWhiteSpace(isbnParcasi))
            {
                ISBNIcerenleriAraRecursive(Kok, isbnParcasi.ToLower(), bulunanlar);
            }
            return bulunanlar;
        }

        // Özyinelemeli ISBN parçası arama
        private void ISBNIcerenleriAraRecursive(BstKitapNode dugum, string isbnParcasi, List<Kitap> bulunanlar)
        {
            if (dugum == null) return;

            ISBNIcerenleriAraRecursive(dugum.Sol, isbnParcasi, bulunanlar);
            
            if (dugum.Veri.ISBN.Contains(isbnParcasi))
            {
                bulunanlar.Add(dugum.Veri);
            }

            ISBNIcerenleriAraRecursive(dugum.Sag, isbnParcasi, bulunanlar);
        }

        // Ağaçtaki tüm kitapları sıralı listeler
        public List<Kitap> InorderListele()
        {
            List<Kitap> kitaplar = new List<Kitap>();
            InorderRecursive(Kok, kitaplar);
            return kitaplar;
        }

        // Özyinelemeli inorder dolaşma
        private void InorderRecursive(BstKitapNode dugum, List<Kitap> liste)
        {
            if (dugum == null) return;
            InorderRecursive(dugum.Sol, liste);
            liste.Add(dugum.Veri);
            InorderRecursive(dugum.Sag, liste);
        }
    }
}
