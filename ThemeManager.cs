// ThemeManager sınıfı, uygulamanın tema (ışık/karanlık) ayarını değiştirmek için kullanılır.
// SetTheme metodu, belirtilen temaya ait ResourceDictionary dosyasını yükleyip uygulamanın tüm stilini dinamik olarak değiştirir.

using System.Windows;

using System.Windows;

namespace KutuphaneYonetimSistemi
{
    public static class ThemeManager
    {
        public static void SetTheme(string theme)
        {
            var dict = new ResourceDictionary();
            switch (theme)
            {
                case "Dark":
                    dict.Source = new System.Uri("Themes/DarkTheme.xaml", System.UriKind.Relative);
                    break;
                default:
                    dict.Source = new System.Uri("Themes/LightTheme.xaml", System.UriKind.Relative);
                    break;
            }

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
    }
}
