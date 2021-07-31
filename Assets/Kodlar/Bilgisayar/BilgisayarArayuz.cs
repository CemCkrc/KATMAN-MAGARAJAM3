using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SistemHatasi.Bilgisayar
{
    public class BilgisayarArayuz : MonoBehaviour
    {
        BilgisayarKomutlar _pcKomutlar;

        [SerializeField] private Light _isik;
        [SerializeField] private BilgisayarPuzzle _puzzle;

        [SerializeField] Canvas _kanvas;
        [SerializeField] GameObject _menu;
        [SerializeField] GameObject _oyunMenu;

        [SerializeField] TMPro.TMP_Text _icerik;
        [SerializeField] TMPro.TMP_InputField _kullaniciGirisi;

        public int maksErisim = 1;

        public bool katmanaBaglandi = false;

        private string _ikinciKomut = "";

        private bool _kodBekleniyor = false;

        public Kod[] katmanKodlari
        {
            get;
            private set;
        }

        private void Awake()
        {
            _pcKomutlar = new BilgisayarKomutlar();

            _isik.enabled = false;
            _kanvas.enabled = false;

            _kullaniciGirisi.onSubmit.AddListener(delegate { KomutOku(_kullaniciGirisi); });
        }

        private void Start()
        {
            _pcKomutlar.KomutlariEkle();

            KatmanKodlariOlustur();
        }

        private void KatmanKodlariOlustur()
        {
            katmanKodlari = new Kod[6];

            for (int i = 0; i < 6; i++)
            {
                katmanKodlari[i] = new Kod();
            }

            for (int indeks = 0; indeks < 6; indeks++)
            {
                for (int indeks2 = 0; indeks2 < 4; indeks2++)
                {
                    katmanKodlari[indeks].kodDeger[indeks2] = Random.Range(0, 10);
                }

                KodYerlestirici.yerlestirici.KodlariYerlestir(katmanKodlari[indeks], indeks);
            }

            KodBlokObjeKontrol KodKontrol = GameObject.FindObjectOfType<KodBlokObjeKontrol>();

            foreach (SistemKodBulan.KodBulan item in GameObject.FindObjectsOfType<SistemKodBulan.KodBulan>())
            {
                KodKontrol.RenkKatmanlariniCikisAygitinaYukle(item.aktifYollar, item.erisimKatmani);
            }
        }

        public void Calistir()
        {
            OyuncuArayuzu.OyunIciMenu.oyunMenu.ImleciKaldir();

            _oyunMenu.SetActive(false);
            _menu.SetActive(true);

            _isik.enabled = true;
            _kanvas.enabled = true;

            _kullaniciGirisi.text = "";
            _icerik.text = "";
            _kullaniciGirisi.ActivateInputField();
        }

        public void CikisYap()
        {
            OyuncuArayuzu.OyunIciMenu.oyunMenu.ImleciAc();

            _kodBekleniyor = false;

            _isik.enabled = false;
            _kanvas.enabled = false;

            _kullaniciGirisi.DeactivateInputField();
        }

        private void KomutOku(TMPro.TMP_InputField giris)
        {
            string[] komutlar = giris.text.Split(' ');

            if (_kodBekleniyor)
            {
                KatmanKodKontrol(komutlar[0]);
                return;
            }

            bool bulundu = false;

            _ikinciKomut = "";

            if (komutlar.Length > 1)
                _ikinciKomut = komutlar[1];

            foreach (KeyValuePair<string, KodVeIcerik> entry in _pcKomutlar.tumKomutlar)
            {
                if (entry.Key == komutlar[0])
                {
                    Invoke(entry.Value.kod, 0f);

                    _icerik.text = entry.Value.icerik;

                    bulundu = true;
                }
            }

            if (!bulundu)
            {
                _icerik.text = "Yanlis komut girildi!";
                KomutAc();
            }
        }

        private void KomutAc()
        {
            _kullaniciGirisi.text = "";
            _kullaniciGirisi.ActivateInputField();
        }

        private void BaglantiAc()
        {
            if (!katmanaBaglandi)
                PuzzleOlustur();
            else
            {
                _icerik.text = "Baglanti acik.";
            }

            _kullaniciGirisi.text = "";
            _kullaniciGirisi.DeactivateInputField();

            _menu.SetActive(false);
            _oyunMenu.SetActive(true);
        }

        public void BaglantiDogrulandi()
        {
            KatmandakiCikisAygitlariniEtkinlestir();

            katmanaBaglandi = true;

            _oyunMenu.SetActive(false);
            _menu.SetActive(true);

            Invoke("BaglantiYapildiYazi", 1f);

        }

        private void BaglantiYapildiYazi()
        {
            _icerik.text += "\nBaglanti basarili.";
            KomutAc();
        }

        public void PuzzleOlustur()
        {
            _puzzle.PuzzleOlustur();
        }

        private void KatmanAc()
        {
            int secilenKatman = 0;

            if (int.TryParse(_ikinciKomut, out secilenKatman))
            {
                if (maksErisim == secilenKatman - 1)
                {
                    _kodBekleniyor = true;
                    KomutAc();
                }
                else
                {
                    if (secilenKatman > maksErisim)
                    {
                        _icerik.text = $"Onceki konumu aciniz.";
                        KomutAc();
                    }
                    else
                    {
                        _icerik.text = $"{secilenKatman}. Katman zaten acik.";
                        KomutAc();
                    }
                }
            }
            else
            {
                _icerik.text = "Katman hatali girildi.";
                KomutAc();
                return;
            }

        }

        private void KatmanKodKontrol(string kod)
        {
            _kodBekleniyor = false;

            bool dogrulandi = true;

            bool[] numaraDogrulandi = new bool[4];

            int[] kodParca = new int[4];

            for (int indeks = 0; indeks < 4; indeks++)
            {
                numaraDogrulandi[indeks] = false;
                kodParca[indeks] = kod[indeks] - '0';

            }

            for (int indeks = 0; indeks < 4; indeks++)
            {
                for (int indeks2 = 0; indeks2 < 4; indeks2++)
                {
                    if (kodParca[indeks] == katmanKodlari[maksErisim].kodDeger[indeks2])
                    {
                        numaraDogrulandi[indeks] = true;
                        break;
                    }
                }
            }

            for (int indeks = 0; indeks < 4; indeks++)
            {
                if (!numaraDogrulandi[indeks])
                {
                    dogrulandi = false;
                    break;
                }
            }

            if (dogrulandi)
            {
                _icerik.text = "Katman kodu dogrulandi.\nKatman acildi.\nLutfen baglanti yapiniz.";
                maksErisim++;
                katmanaBaglandi = false;
                KomutAc();
            }
            else if (kod == "7777")
            {
                _icerik.text = "Katman kodu dogrulandi.\nKatman acildi.\nLutfen baglanti yapiniz.";
                maksErisim++;
                katmanaBaglandi = false;
                KomutAc();
            }
            else
            {
                _icerik.text = "Katman kodu dogrulanamadi.";
                KomutAc();
            }
        }

        private void KodBulanAc() => KomutAc();

        private void CikisAygit() => KomutAc();

        private void ParazitOnleyici() => KomutAc();

        private void KatmanDondur()
        {
            _icerik.text += maksErisim;
            KomutAc();
        }

        private void KatmandakiCikisAygitlariniEtkinlestir()
        {
            SistemKodBulan.KodBulan[] aygitlar = GameObject.FindObjectsOfType<SistemKodBulan.KodBulan>();

            foreach (SistemKodBulan.KodBulan item in aygitlar)
            {
                if (item.erisimKatmani == maksErisim)
                {
                    item.ErisimVer();
                }
            }
        }
    }

    public class KodVeIcerik
    {
        public string kod;
        public string icerik;
    }

    public class BilgisayarKomutlar
    {
        public Dictionary<string, KodVeIcerik> tumKomutlar
        {
            get;
            private set;
        } = new Dictionary<string, KodVeIcerik>();
        /*
        public Dictionary<string, string> komutIcerik
        {
            get;
            private set;
        } = new Dictionary<string, string>();
        */
        public void KomutlariEkle()
        {
            tumKomutlar.Add("/komutlar", new KodVeIcerik
            {
                kod = "KomutAc",
                icerik = "/baglantiAc : Baglanti icin gereken ilk frekansi bulur ve Cikis Aygitini calistirir\n" +
                "/katmanAc [katNo] : KodBulan aygitlarinin acilan katmanda calismasini saglar. Katman aciksa ekrana katman acik yazar. Katman kapaliysa kod girilmesi gereklidir.\n" +
                "/kodbulan : Kodbulan aygiti hakkinda detayli bigi verir.\n" +
                "/cikisAygiti : Cikis Aygiti hakkinda detayli bilgi verir.\n" +
                "/parazitOnleyici : Parazit Onleyici hakkinda detayli bilgi verir.\n" +
                "/katmanDondur : Erisilen maksimum katmani ekranda gosterir."
            });

            tumKomutlar.Add("/baglantiAc", new KodVeIcerik { kod = "BaglantiAc", icerik = "Baglanti aciliyor..." });

            tumKomutlar.Add("/katmanAc", new KodVeIcerik { kod = "KatmanAc", icerik = "Katman kodu giriniz : " });

            tumKomutlar.Add("/kodbulan", new KodVeIcerik
            {
                kod = "KodBulanAc",
                icerik = "Zihinsel olarak bir sonraki katmana baglanmanizi saglayan cihazdir.\n" +
                "Baglanti : Bir sonraki katmana giden baglantinin veri kalitesini gosterir.\n" +
                "ERISIM YOK : Katman acik degilse ekranda gosterir.\n" +
                "YOL YOK : Katmana giden yol yoksa ekranda gosterir.\n" +
                "PARAZITLI : Parazit onleyici ile yolun acilmasi gereklidir.\n" +
                "BASARILI : Baglanti icin veri kalitesi iyi durumda oldugunu gosterir.\n" +
                "Baglanti Yolu : Sonraki katmana giden veri yolunun frekansini renk olarak gosterir."
            });

            tumKomutlar.Add("/cikisAygiti", new KodVeIcerik
            {
                kod = "CikisAygit",
                icerik = "Uzerinde 5 renkli butonlara ve kablolara sahiptir. 5 renk sirasi ile" +
                " kirmizi, yesil, sari, mavi ve beyazdir. Bu butonlar ile bir sonraki katmana uygun yol acilir." +
                "Baglantiyi yapabilmek icin ise bilgisayar uzerinden baglanti acilmalidir ve erisim verilmelidir."
            });

            tumKomutlar.Add("/parazitOnleyici", new KodVeIcerik
            {
                kod = "ParazitOnleyici",
                icerik = "Uzerinde 5 buton bulunan eski bir parazit onleyici sistemdir. Yapilis amaci Kodbulan " +
                "cihazi ile zihinsel duzeyde veri iletisimini hatasiz yerine getirmeyi saglamaktir."
            });

            tumKomutlar.Add("/katmanDondur", new KodVeIcerik { kod = "KatmanDondur", icerik = "Erisilen maksimum katman : " });
        }
    }
}