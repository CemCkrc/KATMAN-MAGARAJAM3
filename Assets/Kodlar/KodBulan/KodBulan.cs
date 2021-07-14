using SistemHatasi.Arayuz;
using SistemHatasi.Oyuncu;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SistemHatasi.SistemKodBulan
{
    public class KodBulan : MonoBehaviour, IEtkilesim
    {
        private AudioSource _sesKaynak;

        public Transform ilkPozisyon, ikinciPozisyon;

        [SerializeField] Transform _kafalikIlkPos;

        public Transform kafalik;

        public ParazitOnleyici.ParazitOnleyici onleyici;

        public CikisAygiti.CikisAygiti cikisAygit;

        public bool[] aktifYollar;

        private OyuncuKontrolu _oyuncu;
        private Transform _oyuncuKamera;

        [SerializeField] private TMP_Text _yolYazi;
        [SerializeField] private Color[] _yolRenkler;

        [SerializeField] private TMP_Text _parazitYazi;
        [SerializeField] private Color[] _parazitRenkler;

        private int _seciliYol = 0;

        public int erisimKatmani = 1;

        public bool erisimVerildi
        {
            get;
            private set;
        } = false;

        private KABLOLAR oncekiKablo;
        private bool _asagiBaglanti = false;
        private bool _girisYapiliyor = false;
        private float adimOrani;
        private float hareketOrani;
        private bool _cikisYapiliyor;

        private void Awake()
        {
            cikisAygit.guncelleme.AddListener(Guncelle);
            onleyici.guncelleme.AddListener(Guncelle);

            oncekiKablo = KABLOLAR.KIRMIZI;

            _sesKaynak = GetComponent<AudioSource>();
        }

        private void Start()
        {
            if (erisimVerildi)
                onleyici.ParazitBaslat();

            _oyuncu = GameObject.FindObjectOfType<OyuncuKontrolu>();
            _oyuncuKamera = _oyuncu.transform.GetChild(0).GetChild(0);

            _kafalikIlkPos.position = kafalik.transform.position;
            _kafalikIlkPos.rotation = kafalik.transform.rotation;
        }

        private void Update()
        {
            if (_girisYapiliyor)
            {
                adimOrani += Time.deltaTime * 0.1f;
                hareketOrani = Mathf.MoveTowards(hareketOrani, 1f, adimOrani);

                if (Tasi(kafalik, _oyuncuKamera.transform, hareketOrani))
                {
                    hareketOrani = 0f;
                    adimOrani = 0f;
                    _girisYapiliyor = false;

                    Isinla();

                    return;
                }
            }

            if (_cikisYapiliyor)
            {
                adimOrani += Time.deltaTime * 0.1f;
                hareketOrani = Mathf.MoveTowards(hareketOrani, 1f, adimOrani);

                if (Tasi(kafalik, _kafalikIlkPos, hareketOrani))
                {
                    hareketOrani = 0f;
                    adimOrani = 0f;
                    _girisYapiliyor = false;

                    CikisYap();

                    return;
                }
            }
        }

        private void Isinla()
        {
            if (_asagiBaglanti)
            {
                _oyuncu.transform.parent = this.transform;

                transform.position = ikinciPozisyon.position;
                transform.rotation = ikinciPozisyon.rotation;

                _oyuncu.transform.parent = null;
            }
            else
            {
                _oyuncu.transform.parent = this.transform;

                transform.position = ilkPozisyon.position;
                transform.rotation = ilkPozisyon.rotation;

                _oyuncu.transform.parent = null;
            }

            _cikisYapiliyor = true;
        }

        private void CikisYap()
        {
            _cikisYapiliyor = false;
            _oyuncu.enabled = true;
        }

        public void Guncelle()
        {
            YolKontrol();
            ParazitKontrol();
        }

        public void ErisimVer()
        {
            erisimVerildi = true;
            if (aktifYollar[_seciliYol])
                onleyici.ParazitBaslat();
            Guncelle();
        }

        private void ParazitKontrol()
        {
            if (!erisimVerildi)
            {
                _parazitYazi.text = "ERISIM YOK";
                _parazitYazi.color = _parazitRenkler[0];
                return;
            }

            if (!onleyici.parazitBasladi)
            {
                if (aktifYollar[_seciliYol])
                {
                    _parazitYazi.text = "BASARILI";
                    _parazitYazi.color = _parazitRenkler[2];
                }
                else
                {
                    _parazitYazi.text = "YOL YOK";
                    _parazitYazi.color = _parazitRenkler[0];
                }
            }
            else
            {
                if (!aktifYollar[_seciliYol])
                {
                    _parazitYazi.text = "YOL YOK";
                    _parazitYazi.color = _parazitRenkler[0];
                }
                else
                {
                    _parazitYazi.text = "PARAZITLI";
                    _parazitYazi.color = _parazitRenkler[1];
                }
            }
        }

        private void YolKontrol()
        {
            switch (cikisAygit.aktifKabloRengi)
            {
                case KABLOLAR.KIRMIZI:
                    if (cikisAygit.aktifKabloRengi != oncekiKablo)
                    {
                        _seciliYol = 0;
                        _yolYazi.text = "KIRMIZI";
                        _yolYazi.color = _yolRenkler[0];

                        if (erisimVerildi && aktifYollar[_seciliYol])
                        {
                            onleyici.ParazitBaslat();
                        }

                        oncekiKablo = cikisAygit.aktifKabloRengi;

                        if (onleyici.parazitBasladi && !aktifYollar[_seciliYol])
                        {
                            onleyici.ParazitDurdur();
                        }
                    }
                    break;
                case KABLOLAR.YESIL:
                    if (cikisAygit.aktifKabloRengi != oncekiKablo)
                    {
                        _seciliYol = 1;
                        _yolYazi.text = "YESIL";
                        _yolYazi.color = _yolRenkler[1];

                        if (erisimVerildi && aktifYollar[_seciliYol])
                            onleyici.ParazitBaslat();

                        oncekiKablo = cikisAygit.aktifKabloRengi;

                        if (onleyici.parazitBasladi && !aktifYollar[_seciliYol])
                        {
                            onleyici.ParazitDurdur();
                        }
                    }
                    break;
                case KABLOLAR.SARI:
                    if (cikisAygit.aktifKabloRengi != oncekiKablo)
                    {
                        _seciliYol = 2;
                        _yolYazi.text = "SARI";
                        _yolYazi.color = _yolRenkler[2];

                        if (erisimVerildi && aktifYollar[_seciliYol])
                            onleyici.ParazitBaslat();

                        oncekiKablo = cikisAygit.aktifKabloRengi;

                        if (onleyici.parazitBasladi && !aktifYollar[_seciliYol])
                        {
                            onleyici.ParazitDurdur();
                        }
                    }
                    break;
                case KABLOLAR.MAVI:
                    if (cikisAygit.aktifKabloRengi != oncekiKablo)
                    {
                        _seciliYol = 3;
                        _yolYazi.text = "MAVI";
                        _yolYazi.color = _yolRenkler[3];

                        if (erisimVerildi && aktifYollar[_seciliYol])
                            onleyici.ParazitBaslat();

                        oncekiKablo = cikisAygit.aktifKabloRengi;

                        if (onleyici.parazitBasladi && !aktifYollar[_seciliYol])
                        {
                            onleyici.ParazitDurdur();
                        }
                    }
                    break;
                case KABLOLAR.BEYAZ:
                    if (cikisAygit.aktifKabloRengi != oncekiKablo)
                    {
                        _seciliYol = 4;
                        _yolYazi.text = "BEYAZ";
                        _yolYazi.color = _yolRenkler[4];

                        if (erisimVerildi && aktifYollar[_seciliYol])
                            onleyici.ParazitBaslat();

                        oncekiKablo = cikisAygit.aktifKabloRengi;

                        if (onleyici.parazitBasladi && !aktifYollar[_seciliYol])
                        {
                            onleyici.ParazitDurdur();
                        }
                    }
                    break;
                default:
                    if (cikisAygit.aktifKabloRengi != oncekiKablo)
                    {
                        _seciliYol = 0;
                        _yolYazi.text = "KIRMIZI";
                        _yolYazi.color = _yolRenkler[0];

                        if (erisimVerildi && aktifYollar[_seciliYol])
                            onleyici.ParazitBaslat();

                        oncekiKablo = cikisAygit.aktifKabloRengi;

                        if (onleyici.parazitBasladi && !aktifYollar[_seciliYol])
                        {
                            onleyici.ParazitDurdur();
                        }
                    }
                    break;
            }
        }

        public void Baglan()
        {
            if (!onleyici.parazitBasladi && aktifYollar[_seciliYol] && erisimVerildi)
            {
                _sesKaynak.PlayOneShot(_sesKaynak.clip);

                _oyuncu.enabled = false;

                _girisYapiliyor = true;

                if (!_asagiBaglanti)
                {
                    GameObject.FindObjectOfType<Bilgisayar.KodBlokObjeKontrol>().ObjeleriBlokla(oncekiKablo, erisimKatmani);
                    _asagiBaglanti = true;
                }
                else
                {
                    GameObject.FindObjectOfType<Bilgisayar.KodBlokObjeKontrol>().ObjeBlokKaldir(oncekiKablo, erisimKatmani);
                    _asagiBaglanti = false;
                }
            }
        }

        private bool Tasi(Transform ilkPozisyon, Transform ikinciPozisyon, float sure)
        {
            ilkPozisyon.position = Vector3.Lerp(ilkPozisyon.position, ikinciPozisyon.position, sure);
            ilkPozisyon.rotation = Quaternion.Lerp(ilkPozisyon.rotation, ikinciPozisyon.rotation, sure);

            if (sure >= 1f)
                return true;
            else
                return false;
        }

        public void Etkilesim()
        {
            Baglan();
        }

        public string EtkilesimAdi()
        {
            return "KODBULAN";
        }
    }

}
