using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace KutuphaneYonetimSistemi
{
    // İşlem yönetim sayfası sınıfı
    public partial class IslemYonetimPage : Page
    {
        // İşlem yığını
        private IslemYigin islemYigin = new IslemYigin();
        // Kitap servisi referansı
        private readonly KitapService _kitapService;

        // Constructor
        public IslemYonetimPage()
        {
            InitializeComponent();
            _kitapService = KitapService.Instance;
        }

        // Yeni işlem ekleme
        private void EkleIslem(string tip)
        {
            if (string.IsNullOrWhiteSpace(txtKitapAdi.Text))
            {
                MessageBox.Show("Lütfen bir kitap adı girin.");
                return;
            }

            Islem islem = new Islem
            {
                KitapAdi = txtKitapAdi.Text,
                Tip = tip,
                Tarih = DateTime.Now
            };

            islemYigin.Ekle(islem);
            ListeyiGuncelle();
        }

        // Ödünç alma butonu click eventi
        private void BtnOduncAl_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKitapAdi.Text))
            {
                MessageBox.Show("Lütfen bir kitap adı girin.");
                return;
            }

            // Kitap kontrolü
            var kitaplar = _kitapService.TumKitaplariGetir();
            bool kitapVar = kitaplar.Any(k => k.Ad.Equals(txtKitapAdi.Text, StringComparison.OrdinalIgnoreCase));

            if (!kitapVar)
            {
                MessageBox.Show("Aradığınız kitap sistemde bulunamamaktadır.");
                return;
            }

            EkleIslem("Ödünç Alındı");
        }

        // İade etme butonu click eventi
        private void BtnIadeEt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKitapAdi.Text))
            {
                MessageBox.Show("Lütfen bir kitap adı girin.");
                return;
            }

            // Kitap kontrolü
            var kitaplar = _kitapService.TumKitaplariGetir();
            var kitap = kitaplar.FirstOrDefault(k => k.Ad.Equals(txtKitapAdi.Text, StringComparison.OrdinalIgnoreCase));

            if (kitap == null)
            {
                MessageBox.Show("İade etmek istediğiniz kitap sistemde bulunamamaktadır.");
                return;
            }

            EkleIslem("İade Edildi");

            // Rezervasyon kontrolü
            RezervasyonPage.KitapIadeEdildi(kitap.ISBN);
        }

        // Geri alma butonu click eventi
        private void BtnGeriAl_Click(object sender, RoutedEventArgs e)
        {
            var geriAlinan = islemYigin.GeriAl();
            if (geriAlinan != null)
            {
                MessageBox.Show($"Geri alındı: {geriAlinan.KitapAdi} ({geriAlinan.Tip})");
            }
            else
            {
                MessageBox.Show("Geri alınacak işlem bulunamadı.");
            }
            ListeyiGuncelle();
        }

        // İşlem listesini güncelleme
        private void ListeyiGuncelle()
        {
            lstIslemler.Items.Clear();
            foreach (var islem in islemYigin.TumIslemler())
            {
                lstIslemler.Items.Add(islem.ToString());
            }
        }
    }
}
