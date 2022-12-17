using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SistemHatasi.Oyuncu
{
    [RequireComponent(typeof(AudioSource))]
    public class OyuncuAyakSesi : MonoBehaviour
    {
        private AudioSource _ayakSesiKaynak;

        [SerializeField] private AudioClip[] _ayakSesleri;

        private float _sesHiz = 1f;

        private void Awake()
        {
            _ayakSesiKaynak = GetComponent<AudioSource>();
        }

        public void SesCalistir(OyuncuHareketTipleri hareketTipi)
        {
            if (_ayakSesiKaynak.isPlaying)
                return;

            switch (hareketTipi)
            {
                case OyuncuHareketTipleri.Hareketsiz:
                    _sesHiz = 0f;
                    break;
                case OyuncuHareketTipleri.Yuruyor:
                    _sesHiz = 0.4f;
                    break;
                case OyuncuHareketTipleri.Kosuyor:
                    _sesHiz = 0.7f;
                    break;
                default:
                    _sesHiz = 0f;
                    break;
            }

            _ayakSesiKaynak.pitch = _sesHiz;

            if (_sesHiz > 0f)
                _ayakSesiKaynak.PlayOneShot(RastgeleSesGetir());
        }



        private AudioClip RastgeleSesGetir()
        {
            return _ayakSesleri[Random.Range(0, _ayakSesleri.Length)];
        }
    }
}

