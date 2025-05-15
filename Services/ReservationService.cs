// ReservationService sınıfı, tüm rezervasyon işlemlerini merkezi şekilde yöneten Singleton yapıda bir hizmet sınıfıdır.
using KutuphaneYonetimSistemi;

public class ReservationService
{
    // Singleton örneği (tek bir nesne)
    private static ReservationService _instance;

    // Eşzamanlı erişimi kontrol altına almak için kilit nesnesi
    private static readonly object _lock = new object();

    // Kitap ISBN’lerine göre rezervasyon kuyruklarını tutan sözlük
    private readonly Dictionary<string, RezervasyonKuyrugu> _rezervasyonKuyruklari;

    // Kitap bilgilerini sağlayan servis (Kitapları ağactan aramak için kullanılır)
    private readonly KitapService _kitapService;

    // Constructor — dışarıdan erişilemez, yalnızca Singleton içinde çağrılır
    private ReservationService()
    {
        _rezervasyonKuyruklari = new Dictionary<string, RezervasyonKuyrugu>();
        _kitapService = KitapService.Instance;
    }

    // ReservationService sınıfının tekil örneğini döner (thread-safe Singleton pattern)
    public static ReservationService Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock) // Çoklu iş parçacığına karşı kilitleme
                {
                    if (_instance == null)
                    {
                        _instance = new ReservationService();
                    }
                }
            }
            return _instance;
        }
    }

    // Kullanıcının belirli bir ISBN’ye sahip kitap için rezervasyon talebini kuyruğa ekler
    public void RezervasyonEkle(string kullaniciAdi, string kitapISBN)
    {
        var kitap = _kitapService.AgactanKitapAra(kitapISBN); // ISBN ile kitap kontrolü
        if (kitap == null)
        {
            throw new Exception("Kitap bulunamadı.");
        }

        // Rezervasyon nesnesi oluşturuluyor
        var rezervasyon = new Rezervasyon
        {
            KullaniciAdi = kullaniciAdi,
            KitapISBN = kitapISBN,
            KitapAdi = kitap.Ad,
            RezervasyonTarihi = DateTime.Now
        };

        // Eğer ISBN'ye ait bir kuyruk yoksa oluştur
        if (!_rezervasyonKuyruklari.ContainsKey(kitapISBN))
        {
            _rezervasyonKuyruklari[kitapISBN] = new RezervasyonKuyrugu();
        }

        // Rezervasyonu kuyruğa ekle (FIFO yapısı)
        _rezervasyonKuyruklari[kitapISBN].Ekle(rezervasyon);
    }

    // Bir kitap iade edildiğinde, ilgili ISBN’ye ait rezervasyon kuyruğundan ilk rezervasyonu çıkarır (önceliklendirme)
    public Rezervasyon KitapIadeEdildi(string kitapISBN)
    {
        if (!_rezervasyonKuyruklari.ContainsKey(kitapISBN) || _rezervasyonKuyruklari[kitapISBN].BosMu())
        {
            return null;
        }

        return _rezervasyonKuyruklari[kitapISBN].Cikar();
    }

    // Belirli bir ISBN’ye ait tüm rezervasyonları listeler
    public List<Rezervasyon> KitapRezervasyonlariniGetir(string kitapISBN)
    {
        if (!_rezervasyonKuyruklari.ContainsKey(kitapISBN))
        {
            return new List<Rezervasyon>();
        }

        return _rezervasyonKuyruklari[kitapISBN].TumunuListele();
    }

    // Tüm kitaplar için yapılmış tüm rezervasyonları birleştirerek tarih sırasına göre döner
    public List<Rezervasyon> TumRezervasyonlariGetir()
    {
        return _rezervasyonKuyruklari.Values
            .SelectMany(kuyruk => kuyruk.TumunuListele())
            .OrderBy(r => r.RezervasyonTarihi)
            .ToList();
    }

    // Belirli bir ISBN’ye sahip kitap için aktif bir rezervasyon olup olmadığını kontrol eder
    public bool RezervasyonVarMi(string kitapISBN)
    {
        return _rezervasyonKuyruklari.ContainsKey(kitapISBN) && !_rezervasyonKuyruklari[kitapISBN].BosMu();
    }

    // Belirtilen ISBN’ye ait sıradaki (ilk) rezervasyonu döner (silmeden)
    public Rezervasyon SiradakiRezervasyonuGetir(string kitapISBN)
    {
        if (!_rezervasyonKuyruklari.ContainsKey(kitapISBN))
        {
            return null;
        }

        return _rezervasyonKuyruklari[kitapISBN].Siradaki();
    }
}
