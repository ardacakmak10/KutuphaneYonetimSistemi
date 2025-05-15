using System;
using System.Windows;
using System.Windows.Controls;

namespace KutuphaneYonetimSistemi
{
    // Kitap ağacı görüntüleme ve arama sayfası
    public partial class KitapAgaciPage : Page
    {
        private readonly KitapService _kitapService;

        // Sayfa yapıcı metodu - Kitap servisini başlatır ve kitap listesini yükler
        public KitapAgaciPage()
        {
            InitializeComponent();
            _kitapService = KitapService.Instance;
            ListeyiGuncelle(); // Başlangıçta sıralı listeyi göster
        }

        // Ağaçtaki kitapları sıralı şekilde listbox'a yükler
        private void ListeyiGuncelle()
        {
            lstKitaplar.Items.Clear();
            foreach (var kitap in _kitapService.AgactanTumKitaplariGetir()) // Inorder listeleme
            {
                lstKitaplar.Items.Add(kitap.ToString());
            }
        }

        // ISBN arama kutusundaki metin değiştiğinde çalışır
        // Girilen ISBN parçasını içeren kitapları listeler
        private void TxtArananISBN_TextChanged(object sender, TextChangedEventArgs e)
        {
            string isbnParcasi = txtArananISBN.Text;
            
            if (string.IsNullOrWhiteSpace(isbnParcasi))
            {
                ListeyiGuncelle();
                return;
            }

            var sonuclar = _kitapService.ISBNIcerenleriGetir(isbnParcasi);
            lstKitaplar.Items.Clear();

            foreach (var kitap in sonuclar)
            {
                lstKitaplar.Items.Add(kitap.ToString());
            }
        }
    }
}
