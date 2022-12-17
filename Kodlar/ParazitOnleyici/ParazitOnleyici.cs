using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SistemHatasi.ParazitOnleyici
{
    [RequireComponent(typeof(AudioSource), typeof(Animator))]
    public class ParazitOnleyici : MonoBehaviour
    {
        private AudioSource _parazitSesKaynagi;

        private Animator _parazitAnimator;

        [SerializeField] private AudioClip _parazitSes;
        [SerializeField] private AudioClip _parazitBulundu;
        [SerializeField] private AudioClip _parazitBulunamadi;

        private Vector2Int _oyuncuPozisyon;

        private Vector2Int _cozumPozisyon;

        private bool _tuslarAcildi = false;

        public bool parazitBasladi
        {
            get;
            private set;
        } = false;

        [SerializeField] private Transform _oyuncuKonum;

        private Transform[,] _pozisyonlar;

        [SerializeField] private Transform[] _kuyrukPozisyonlar;
        private bool[,] _pozisyonKontrol;

        [SerializeField] private int _pozisyonlarX, _pozisyonlarY;

        public UnityEvent guncelleme;

        private void Awake()
        {
            _parazitSesKaynagi = GetComponent<AudioSource>();
            _parazitAnimator = GetComponent<Animator>();

            _pozisyonKontrol = new bool[_pozisyonlarX, _pozisyonlarY];
            _pozisyonlar = new Transform[_pozisyonlarX, _pozisyonlarY];

            int indeks = 0;

            for (int x = 0; x < _pozisyonlarX; x++)
            {
                for (int y = 0; y < _pozisyonlarY; y++)
                {
                    _pozisyonlar[x, y] = _kuyrukPozisyonlar[indeks];

                    _pozisyonKontrol[x, y] = false;

                    indeks++;
                }
            }

            _oyuncuPozisyon = Vector2Int.zero;
        }

        private void Start()
        {
            guncelleme.Invoke();
        }

        private void Update()
        {
            if (!parazitBasladi)
                return;

            if (!_parazitSesKaynagi.isPlaying)
            {
                _parazitSesKaynagi.PlayOneShot(_parazitSes);
            }
        }

        private void TuslarAcildi()
        {
            _tuslarAcildi = true;
        }

        public void YeniPozisyonSec()
        {
            for (int x = 0; x < _pozisyonlarX; x++)
            {
                for (int y = 0; y < _pozisyonlarY; y++)
                {
                    _pozisyonKontrol[x, y] = false;
                }
            }

            _cozumPozisyon = yeniPozisyon();

            _pozisyonKontrol[_cozumPozisyon.x, _cozumPozisyon.y] = true;

            _parazitSesKaynagi.pitch = yakinlikKontrol();

        }

        public void ParazitBaslat()
        {
            YeniPozisyonSec();

            parazitBasladi = true;
            _tuslarAcildi = true;
        }

        public void ParazitDurdur()
        {
            parazitBasladi = false;
            _tuslarAcildi = false;
        }

        public void ParazitCozumKontrol()
        {
            if (!parazitBasladi)
                return;

            if (!_tuslarAcildi)
                return;

            _parazitAnimator.SetTrigger("OrtaTus");

            _tuslarAcildi = false;
            Invoke("TuslarAcildi", 0.5f);

            if (_pozisyonKontrol[_oyuncuPozisyon.x,_oyuncuPozisyon.y])
            {
                ParazitDurdur();

                _parazitSesKaynagi.PlayOneShot(_parazitBulundu);
            }
            else
            {
                _parazitSesKaynagi.Stop();

                _parazitSesKaynagi.PlayOneShot(_parazitBulunamadi);
            }

            guncelleme.Invoke();
        }

        public void SolTusBasma()
        {
            if (!parazitBasladi)
                return;

            if (!_tuslarAcildi)
                return;

            _parazitAnimator.SetTrigger("SolTus");

            if(_oyuncuPozisyon.y != 0)
            {
                _oyuncuPozisyon.y--;

                _oyuncuKonum.position = _pozisyonlar[_oyuncuPozisyon.x, _oyuncuPozisyon.y].position;
            }

            _parazitSesKaynagi.pitch = yakinlikKontrol();

            _tuslarAcildi = false;
            Invoke("TuslarAcildi", 0.5f);
        }

        public void SagTusBasma()
        {
            if (!parazitBasladi)
                return;

            if (!_tuslarAcildi)
                return;

            _parazitAnimator.SetTrigger("SagTus");

            if (_oyuncuPozisyon.y != _pozisyonlarY - 1)
            {
                _oyuncuPozisyon.y++;

                _oyuncuKonum.position = _pozisyonlar[_oyuncuPozisyon.x, _oyuncuPozisyon.y].position;
            }

            _parazitSesKaynagi.pitch = yakinlikKontrol();

            _tuslarAcildi = false;
            Invoke("TuslarAcildi", 0.5f);

        }

        public void YukariTusBasma()
        {
            if (!parazitBasladi)
                return;

            if (!_tuslarAcildi)
                return;

            _parazitAnimator.SetTrigger("YukariTus");

            if (_oyuncuPozisyon.x != 0)
            {
                _oyuncuPozisyon.x--;

                _oyuncuKonum.position = _pozisyonlar[_oyuncuPozisyon.x, _oyuncuPozisyon.y].position;
            }

            _parazitSesKaynagi.pitch = yakinlikKontrol();

            _tuslarAcildi = false;
            Invoke("TuslarAcildi", 0.5f);
        }

        public void AsagiTusBasma()
        {
            if (!parazitBasladi)
                return;

            if (!_tuslarAcildi)
                return;

            _parazitAnimator.SetTrigger("AsagiTus");

            if (_oyuncuPozisyon.x != _pozisyonlarX - 1)
            {
                _oyuncuPozisyon.x++;

                _oyuncuKonum.position = _pozisyonlar[_oyuncuPozisyon.x, _oyuncuPozisyon.y].position;
            }

            _parazitSesKaynagi.pitch = yakinlikKontrol();

            _tuslarAcildi = false;
            Invoke("TuslarAcildi", 0.5f);
        }

        public float yakinlikKontrol()
        {
            float sesHiz = 0;
            int yakinlik = 0;

            int xYakinlik = Mathf.Abs(_oyuncuPozisyon.x - _cozumPozisyon.x);
            int yYakinlik = Mathf.Abs(_oyuncuPozisyon.y - _cozumPozisyon.y);

            yakinlik = xYakinlik + yYakinlik;

            if(yakinlik > 2)
            {
                sesHiz = 0.3f;
            }
            else if (yakinlik == 2)
            {
                sesHiz = 1f;
            }
            else if (yakinlik == 1)
            {
                sesHiz = 2f;
            }
            else if(yakinlik == 0)
            {
                sesHiz = 3f;
            }

            return sesHiz;
        }

        public Vector2Int yeniPozisyon()
        {
            int pozisyonX = Random.Range(0, _pozisyonlarX);
            int pozisyonY = Random.Range(0, _pozisyonlarY);

            if(pozisyonX == _oyuncuPozisyon.x && pozisyonY == _oyuncuPozisyon.y)
            {
                return yeniPozisyon();
            }
            else
            {
                return new Vector2Int(pozisyonX, pozisyonY);
            }
        }
    }
    public enum Yonler
    {
        YUKARI,
        ASAGI,
        SOL,
        SAG,
        ORTA
    }
}
