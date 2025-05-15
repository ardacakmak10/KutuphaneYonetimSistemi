namespace KutuphaneYonetimSistemi
{
    // Bağlı listede kitapları tutan düğüm sınıfı
    public class KitapNode
    {
        // Düğümde saklanan kitap nesnesi
        public Kitap Veri { get; set; }

        // Bağlı listedeki bir sonraki düğüm
        public KitapNode Sonraki { get; set; }

        // Verilen kitap nesnesiyle yeni bir düğüm oluşturur ve sonraki bağlantıyı null olarak başlatır
        public KitapNode(Kitap veri)
        {
            Veri = veri;
            Sonraki = null;
        }
    }
}
