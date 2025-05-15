using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace KutuphaneYonetimSistemi
{
    // Kitap yönetim sayfası sınıfı
    public partial class KitapYonetimPage : Page
    {
        // Kitap servisi referansı
        private readonly KitapService _kitapService;

        // Constructor
        public KitapYonetimPage()
        {
            InitializeComponent();
            _kitapService = KitapService.Instance;
        }

        // Form verilerinden kitap nesnesi oluşturma
        private Kitap KitapOlustur()
        {
            return new Kitap
            {
                Ad = txtAd.Text,
                ISBN = txtISBN.Text,
                Yazar = txtYazar.Text,
                YayinYili = int.TryParse(txtYayinYili.Text, out int yil) ? yil : 0
            };
        }

        // Kitap listesini güncelleme
        private void ListeyiGuncelle()
        {
            lstKitaplar.Items.Clear();
            foreach (var kitap in _kitapService.TumKitaplariGetir())
            {
                lstKitaplar.Items.Add(kitap.ToString());
            }
        }

        // Başa ekleme butonu click eventi
        private void BtnBasaEkle_Click(object sender, RoutedEventArgs e)
        {
            _kitapService.BasaEkle(KitapOlustur());
            MessageBox.Show("Kitap başa eklendi.");
            ListeyiGuncelle();
        }

        // Sona ekleme butonu click eventi
        private void BtnSonaEkle_Click(object sender, RoutedEventArgs e)
        {
            _kitapService.SonaEkle(KitapOlustur());
            MessageBox.Show("Kitap sona eklendi.");
            ListeyiGuncelle();
        }

        // ISBN bilgisini string içinden ayıklama
        private string AyiklaISBN(string kitapBilgisi)
        {
            try
            {
                int isbnBaslangic = kitapBilgisi.IndexOf("ISBN: ") + 6;
                int isbnBitis = kitapBilgisi.IndexOf(" - ", isbnBaslangic);
                if (isbnBitis == -1) // Eğer " - " bulunamazsa sonuna kadar al
                {
                    isbnBitis = kitapBilgisi.IndexOf(" (", isbnBaslangic);
                }
                return kitapBilgisi.Substring(isbnBaslangic, isbnBitis - isbnBaslangic);
            }
            catch
            {
                return string.Empty;
            }
        }

        // Filtreleme butonu click eventi
        private void BtnFiltrele_Click(object sender, RoutedEventArgs e)
        {
            string ad = txtAd.Text.Trim();
            string isbn = txtISBN.Text.Trim();
            string yazar = txtYazar.Text.Trim();
            string yayinYili = txtYayinYili.Text.Trim();

            // Hiçbir filtreleme kriteri girilmemişse tüm listeyi göster
            if (string.IsNullOrWhiteSpace(ad) && string.IsNullOrWhiteSpace(isbn) && 
                string.IsNullOrWhiteSpace(yazar) && string.IsNullOrWhiteSpace(yayinYili))
            {
                ListeyiGuncelle();
                return;
            }

            lstKitaplar.Items.Clear();

            var filtrelenmisKitaplar = _kitapService.KitaplariFiltrele(ad, isbn, yazar, yayinYili);
            bool kitapBulundu = false;

            foreach (var kitap in filtrelenmisKitaplar)
            {
                lstKitaplar.Items.Add(kitap.ToString());
                kitapBulundu = true;
            }

            if (!kitapBulundu)
            {
                MessageBox.Show("Aradığınız kriterlere uyan kitap bulunamadı.");
                ListeyiGuncelle(); // Filtreleme sonucu boşsa tüm listeyi göster
            }
        }

        // Kitap silme butonu click eventi
        private void BtnSil_Click(object sender, RoutedEventArgs e)
        {
            if (lstKitaplar.SelectedItem == null)
            {
                MessageBox.Show("Lütfen silinecek kitabı seçin.");
                return;
            }

            string isbn = AyiklaISBN(lstKitaplar.SelectedItem.ToString());
            if (string.IsNullOrWhiteSpace(isbn))
            {
                MessageBox.Show("Seçilen kitabın ISBN bilgisi ayıklanamadı.");
                return;
            }

            var result = MessageBox.Show("Seçili kitabı silmek istediğinizden emin misiniz?", 
                                       "Silme Onayı", 
                                       MessageBoxButton.YesNo, 
                                       MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _kitapService.Sil(isbn);
                MessageBox.Show("Kitap başarıyla silindi.");
                ListeyiGuncelle();
            }
        }

        // Araya ekleme butonu click eventi
        private void BtnArayaEkle_Click(object sender, RoutedEventArgs e)
        {
            // Form kontrolü
            if (string.IsNullOrWhiteSpace(txtAd.Text) || 
                string.IsNullOrWhiteSpace(txtISBN.Text) || 
                string.IsNullOrWhiteSpace(txtYazar.Text) || 
                string.IsNullOrWhiteSpace(txtYayinYili.Text))
            {
                MessageBox.Show("Lütfen tüm kitap bilgilerini doldurun.");
                return;
            }

            // Seçim kontrolü
            if (lstKitaplar.SelectedItems.Count != 2)
            {
                MessageBox.Show("Lütfen listeden tam olarak 2 kitap seçin. Birden fazla kitap seçmek için Ctrl tuşuna basılı tutarak tıklayabilirsiniz.");
                return;
            }

            // ISBN bilgilerini alma
            string isbn1 = AyiklaISBN(lstKitaplar.SelectedItems[0].ToString());
            string isbn2 = AyiklaISBN(lstKitaplar.SelectedItems[1].ToString());

            if (string.IsNullOrWhiteSpace(isbn1) || string.IsNullOrWhiteSpace(isbn2))
            {
                MessageBox.Show("Seçilen kitaplardan birinin ISBN bilgisi ayıklanamadı.");
                return;
            }

            // ISBN kontrolü
            var yeniKitap = KitapOlustur();
            var mevcutKitaplar = _kitapService.TumKitaplariGetir();
            if (mevcutKitaplar.Any(k => k.ISBN == yeniKitap.ISBN))
            {
                MessageBox.Show("Bu ISBN numarası zaten kullanımda. Lütfen benzersiz bir ISBN numarası girin.");
                return;
            }

            try
            {
                // Araya ekleme işlemi
                _kitapService.ArayaEkle(yeniKitap, isbn1, isbn2);
                MessageBox.Show($"'{yeniKitap.Ad}' adlı kitap seçili kitapların arasına başarıyla eklendi.");
                
                // UI güncelleme
                ListeyiGuncelle();
                txtAd.Clear();
                txtISBN.Clear();
                txtYazar.Clear();
                txtYayinYili.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kitap eklenirken bir hata oluştu: {ex.Message}");
            }
        }
    }
}
