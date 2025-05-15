using System;
using System.Collections.Generic;

namespace KutuphaneYonetimSistemi
{
    // Kitapları bağlı liste veri yapısında saklayan sınıf
    public class BagliListe
    {
        // Bağlı listenin baş düğümü
        public KitapNode Bas;

        // Yeni bir kitabı bağlı listenin başına ekler
        public void BasaEkle(Kitap kitap)
        {
            KitapNode yeni = new KitapNode(kitap);
            yeni.Sonraki = Bas;
            Bas = yeni;
        }

        // Yeni bir kitabı bağlı listenin sonuna ekler
        public void SonaEkle(Kitap kitap)
        {
            KitapNode yeni = new KitapNode(kitap);
            if (Bas == null)
            {
                Bas = yeni;
                return;
            }

            KitapNode temp = Bas;
            while (temp.Sonraki != null)
                temp = temp.Sonraki;
            temp.Sonraki = yeni;
        }

        // Yeni bir kitabı verilen iki ISBN numarasına sahip kitapların arasına ekler
        public void ArayaEkle(Kitap yeniKitap, string isbn1, string isbn2)
        {
            if (Bas == null || isbn1 == isbn2)
                return;

            KitapNode onceki = null;
            KitapNode mevcut = Bas;
            KitapNode ilkKitap = null;
            KitapNode ikinciKitap = null;

            // İlk ve ikinci kitabı bul
            while (mevcut != null)
            {
                if (mevcut.Veri.ISBN == isbn1)
                    ilkKitap = mevcut;
                else if (mevcut.Veri.ISBN == isbn2)
                    ikinciKitap = mevcut;
                
                if (ilkKitap != null && ikinciKitap != null)
                    break;
                    
                mevcut = mevcut.Sonraki;
            }

            // Her iki kitap da bulunmazsa işlem yapma
            if (ilkKitap == null || ikinciKitap == null)
                return;

            // Yeni düğümü oluştur
            KitapNode yeniNode = new KitapNode(yeniKitap);

            // İlk kitaptan sonra ekle
            yeniNode.Sonraki = ilkKitap.Sonraki;
            ilkKitap.Sonraki = yeniNode;
        }

        // Bağlı listedeki tüm kitapları liste olarak döndürür
        public List<Kitap> Listele()
        {
            List<Kitap> kitaplar = new List<Kitap>();
            KitapNode temp = Bas;
            while (temp != null)
            {
                kitaplar.Add(temp.Veri);
                temp = temp.Sonraki;
            }
            return kitaplar;
        }

        // Verilen yazar adına göre kitapları filtreler ve bulunanları liste olarak döndürür
        public List<Kitap> Filtrele(string yazar)
        {
            List<Kitap> filtreli = new List<Kitap>();
            KitapNode temp = Bas;
            while (temp != null)
            {
                if (temp.Veri.Yazar.ToLower().Contains(yazar.ToLower()))
                    filtreli.Add(temp.Veri);
                temp = temp.Sonraki;
            }
            return filtreli;
        }

        // Verilen ISBN numarasına sahip kitabı bağlı listeden siler
        public void Sil(string isbn)
        {
            if (Bas == null)
                return;

            if (Bas.Veri.ISBN == isbn)
            {
                Bas = Bas.Sonraki;
                return;
            }

            KitapNode onceki = Bas;
            KitapNode mevcut = Bas.Sonraki;

            while (mevcut != null)
            {
                if (mevcut.Veri.ISBN == isbn)
                {
                    onceki.Sonraki = mevcut.Sonraki;
                    return;
                }
                onceki = mevcut;
                mevcut = mevcut.Sonraki;
            }
        }
    }
}
