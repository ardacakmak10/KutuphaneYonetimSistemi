namespace KutuphaneYonetimSistemi
{
    // İkili arama ağacında kitapları tutan düğüm sınıfı
    public class BstKitapNode
    {
        // Düğümde saklanan kitap nesnesi
        public Kitap Veri { get; set; }

        // Sol alt ağacın kök düğümü (daha küçük ISBN'li kitaplar)
        public BstKitapNode Sol { get; set; }

        // Sağ alt ağacın kök düğümü (daha büyük ISBN'li kitaplar)
        public BstKitapNode Sag { get; set; }

        // Verilen kitap nesnesiyle yeni bir düğüm oluşturur ve sol/sağ bağlantıları null olarak başlatır
        public BstKitapNode(Kitap kitap)
        {
            Veri = kitap;
        }
    }
}
