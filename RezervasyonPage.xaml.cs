using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;

namespace KutuphaneYonetimSistemi
{
    // Rezervasyon sayfası sınıfı
    public partial class RezervasyonPage : Page
    {
        // Kitap servisi referansı
        private readonly KitapService _kitapService;
        // Rezervasyon kuyruğu - ISBN'ye göre kullanıcıları tutar
        private static Dictionary<string, Queue<string>> _rezervasyonKuyrugu = new Dictionary<string, Queue<string>>();
        // Mevcut sayfa örneği
        private static RezervasyonPage _currentInstance;

        // Constructor
        public RezervasyonPage()
        {
            InitializeComponent();
            _kitapService = KitapService.Instance;
            _currentInstance = this;
            ListeyiGuncelle();
        }

        // Yeni rezervasyon ekleme butonu click eventi
        private void BtnEkle_Click(object sender, RoutedEventArgs e)
        {
            // Form kontrolü
            if (string.IsNullOrWhiteSpace(txtKullaniciAdi.Text) || string.IsNullOrWhiteSpace(txtKitapISBN.Text))
            {
                MessageBox.Show("Lütfen kullanıcı adı ve ISBN bilgilerini girin.");
                return;
            }

            string isbn = txtKitapISBN.Text.Trim();
            string kullaniciAdi = txtKullaniciAdi.Text.Trim();

            // Kitap kontrolü
            var kitap = _kitapService.TumKitaplariGetir().FirstOrDefault(k => k.ISBN == isbn);
            if (kitap == null)
            {
                MessageBox.Show("Belirtilen ISBN'ye sahip kitap sistemde bulunamadı.");
                return;
            }

            // Kuyruk kontrolü ve oluşturma
            if (!_rezervasyonKuyrugu.ContainsKey(isbn))
            {
                _rezervasyonKuyrugu[isbn] = new Queue<string>();
            }

            // Mükerrer rezervasyon kontrolü
            if (_rezervasyonKuyrugu[isbn].Contains(kullaniciAdi))
            {
                MessageBox.Show("Bu kullanıcı zaten bu kitap için rezervasyon kuyruğunda.");
                return;
            }

            // Kuyruğa ekleme
            _rezervasyonKuyrugu[isbn].Enqueue(kullaniciAdi);
            MessageBox.Show($"Rezervasyon başarıyla eklendi. Sıra numaranız: {_rezervasyonKuyrugu[isbn].Count}");

            // UI güncelleme
            ListeyiGuncelle();
            txtKullaniciAdi.Clear();
            txtKitapISBN.Clear();
        }

        // Kitap iade edildiğinde çağrılan metod
        public static void KitapIadeEdildi(string isbn)
        {
            // Kuyruk kontrolü
            if (_rezervasyonKuyrugu.ContainsKey(isbn) && _rezervasyonKuyrugu[isbn].Count > 0)
            {
                // Sıradaki kullanıcıyı al
                string siradakiKullanici = _rezervasyonKuyrugu[isbn].Dequeue();
                
                // Boş kuyruk kontrolü
                if (_rezervasyonKuyrugu[isbn].Count == 0)
                {
                    _rezervasyonKuyrugu.Remove(isbn);
                }
                
                MessageBox.Show($"Kitap iade edildi. {siradakiKullanici} kullanıcısına kitap otomatik olarak atandı.");
                
                // UI güncelleme
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _currentInstance?.ListeyiGuncelle();
                });
            }
        }

        // Rezervasyon listesini güncelleme
        private void ListeyiGuncelle()
        {
            lstRezervasyonlar.Items.Clear();
            // Her kitap için rezervasyon bilgilerini listele
            foreach (var kuyruk in _rezervasyonKuyrugu)
            {
                var kitap = _kitapService.TumKitaplariGetir().FirstOrDefault(k => k.ISBN == kuyruk.Key);
                if (kitap != null)
                {
                    lstRezervasyonlar.Items.Add($"Kitap: {kitap.Ad} (ISBN: {kuyruk.Key})");
                    int siraNo = 1;
                    foreach (var kullanici in kuyruk.Value)
                    {
                        lstRezervasyonlar.Items.Add($"   {siraNo}. Sıra: {kullanici}");
                        siraNo++;
                    }
                    lstRezervasyonlar.Items.Add("");
                }
            }
        }
    }
}
