using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SistemHatasi.CikisAygiti
{
    [RequireComponent(typeof(AudioSource))]
    public class CikisAygiti : MonoBehaviour
    {
        public KABLOLAR aktifKabloRengi;

        [SerializeField] private Transform[] _kablolar;

        [SerializeField] private Material[] _butonMateryalleri;

        [SerializeField] private AudioClip _kabloTakma;

        private AudioSource _kabloSesKaynak;

        public int baslangicKablosu = 0;

        private int _aktifKabloNumara = 0;

        public UnityEvent guncelleme;

        private void Awake()
        {
            _kabloSesKaynak = GetComponent<AudioSource>();

            for (int indeks = 0; indeks < _kablolar.Length; indeks++)
            {
                if(indeks == baslangicKablosu)
                {
                    _kablolar[indeks].gameObject.SetActive(true);
                    _butonMateryalleri[indeks].EnableKeyword("_EMISSION");
                    aktifKabloRengi = kabloDondur(indeks);
                    _aktifKabloNumara = indeks;
                }
                else
                {
                    _kablolar[indeks].gameObject.SetActive(false);
                    _butonMateryalleri[indeks].DisableKeyword("_EMISSION");
                }
            }
        }

        private void Start()
        {
            guncelleme?.Invoke();
        }

        public void KabloDegistir(int kabloNumara)
        {
            _kablolar[_aktifKabloNumara].gameObject.SetActive(false);
            _butonMateryalleri[_aktifKabloNumara].DisableKeyword("_EMISSION");

            _kablolar[kabloNumara].gameObject.SetActive(true);
            _butonMateryalleri[kabloNumara].EnableKeyword("_EMISSION");
            aktifKabloRengi = kabloDondur(kabloNumara);
            _aktifKabloNumara = kabloNumara;

            _kabloSesKaynak.PlayOneShot(_kabloTakma);

            guncelleme?.Invoke();
        }

        public void Yenile()
        {
            KabloDegistir(_aktifKabloNumara);

            guncelleme?.Invoke();
        }

        private KABLOLAR kabloDondur(int sira)
        {
            KABLOLAR kablo;

            switch (sira)
            {
                case 0:
                    kablo = KABLOLAR.KIRMIZI;
                    break;
                case 1:
                    kablo = KABLOLAR.YESIL;
                    break;
                case 2:
                    kablo = KABLOLAR.SARI;
                    break;
                case 3:
                    kablo = KABLOLAR.MAVI;
                    break;
                case 4:
                    kablo = KABLOLAR.BEYAZ;
                    break;
                default:
                    kablo = KABLOLAR.KIRMIZI;
                    break;
            }

            return kablo;
        }

    }
}

public enum KABLOLAR
{
    KIRMIZI,
    YESIL,
    SARI,
    MAVI,
    BEYAZ
}