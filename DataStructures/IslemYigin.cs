using System.Collections.Generic;

namespace KutuphaneYonetimSistemi
{
    // Kitap işlemlerini LIFO (son giren ilk çıkar) mantığıyla saklayan yığın sınıfı
    public class IslemYigin
    {
        // İşlemleri tutan yığın veri yapısı
        private Stack<Islem> yigin = new Stack<Islem>();

        // Verilen işlemi yığının en üstüne ekler (son giren)
        public void Ekle(Islem islem)
        {
            yigin.Push(islem);
        }

        // En son eklenen işlemi yığından çıkarır ve döndürür (ilk çıkar)
        // Eğer yığın boşsa null değer döndürür
        public Islem GeriAl()
        {
            return yigin.Count > 0 ? yigin.Pop() : null;
        }

        // Yığındaki tüm işlemleri en yeniden en eskiye doğru sıralı bir liste olarak döndürür
        public List<Islem> TumIslemler()
        {
            return new List<Islem>(yigin);
        }
    }
}
