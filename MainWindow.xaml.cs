using System.Windows;
using System.Windows.Controls;

namespace KutuphaneYonetimSistemi
{
    public partial class MainWindow : Window
    {
        // Sayfaları önceden oluşturarak bellek içinde saklıyoruz
        private KitapYonetimPage kitapYonetimPage = new KitapYonetimPage();
        private IslemYonetimPage islemYonetimPage = new IslemYonetimPage();
        private RezervasyonPage rezervasyonPage = new RezervasyonPage();
        private KitapAgaciPage kitapAgaciPage = new KitapAgaciPage();
        private AboutPage aboutPage = new AboutPage();

        // Ana pencere yapıcı metodu - Arayüzü başlatır ve kitap yönetim sayfasını gösterir
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(kitapYonetimPage); // Uygulama açıldığında Kitap sayfası açılsın
        }

        // Kitap yönetimi sayfasına geçiş yapar
        private void BtnKitapYonetimi_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(kitapYonetimPage);
        }

        // İşlem geçmişi sayfasına geçiş yapar
        private void BtnIslemGecmisi_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(islemYonetimPage);
        }

        // Rezervasyon sayfasına geçiş yapar
        private void BtnRezervasyon_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(rezervasyonPage);
        }

        // Kitap ağacı görüntüleme sayfasına geçiş yapar
        private void BtnKitapAgaci_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(kitapAgaciPage);
        }

        // Hakkında sayfasına geçiş yapar
        private void BtnHakkinda_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(aboutPage);
        }

        private bool koyuTemaAktif = false;

        // Uygulama temasını açık/koyu olarak değiştirir
        private void BtnTema_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary yeniTema = new ResourceDictionary();
            if (koyuTemaAktif)
            {
                // Açık temaya geç
                yeniTema.Source = new Uri("Themes/LightTheme.xaml", UriKind.Relative);
                btnTemaDegistir.Content = "🌞 Koyu Tema";
            }
            else
            {
                // Koyu temaya geç
                yeniTema.Source = new Uri("Themes/DarkTheme.xaml", UriKind.Relative);
                btnTemaDegistir.Content = "🌙 Açık Tema";
            }

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(yeniTema);

            koyuTemaAktif = !koyuTemaAktif;
        }
    }
}
