namespace KutuphaneYonetimSistemi
{
    // Kitap sınıfı - kitap bilgilerini tutar
    public class Kitap
    {
        // Kitabın adı
        public string Ad { get; set; }
        
        // ISBN numarası
        public string ISBN { get; set; }
        
        // Kitabın yazarı
        public string Yazar { get; set; }
        
        // Yayın yılı
        public int YayinYili { get; set; }

        // Kitap bilgilerini string olarak döndürür
        public override string ToString()
        {
            return $"{Ad} - ISBN: {ISBN} - {Yazar} ({YayinYili})";
        }
    }
}
