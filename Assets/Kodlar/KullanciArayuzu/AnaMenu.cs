using SistemHatasi.Denetleyici;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SistemHatasi.OyuncuArayuzu
{
    public class AnaMenu : MenuDenetleyici
    {
        [SerializeField] Slider _sesSlider, _muzikSlider, _fareSlider;
        public void OyunuBaslat()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }

        public void MuzikDegistir(float deger)
        {
            GameManager.oyunDenetleyici.MuzikDegistir(deger);
        }

        public void SesDegistir(float deger)
        {
            GameManager.oyunDenetleyici.SesDegistir(deger);
        }
        public void FareHassasiyetDegistir(float deger)
        {
            GameManager.oyunDenetleyici.FareHassasiyetDegistir(deger);
        }

        public void AyarlarAc()
        {
            float sesDeger = 40f;
            float muzikDeger = 40f;

            float fareHassasiyet = 100f;

            if (PlayerPrefs.HasKey("ses"))
                sesDeger = PlayerPrefs.GetFloat("ses");
            if (PlayerPrefs.HasKey("muzik"))
                muzikDeger = PlayerPrefs.GetFloat("muzik");
            if (PlayerPrefs.HasKey("fare"))
                fareHassasiyet = PlayerPrefs.GetFloat("fare");

            _sesSlider.value = sesDeger;
            _muzikSlider.value = muzikDeger;
            _fareSlider.value = fareHassasiyet;
        }

        public void Onayla()
        {
            PlayerPrefs.SetFloat("ses", _sesSlider.value);
            PlayerPrefs.SetFloat("muzik", _muzikSlider.value);
            PlayerPrefs.SetFloat("fare", _fareSlider.value);
        }

        public void EskiAyarlar()
        {
            float sesDeger = 40f;
            float muzikDeger = 40f;

            float fareHassasiyet = 100f;

            if (PlayerPrefs.HasKey("ses"))
                sesDeger = PlayerPrefs.GetFloat("ses");
            if(PlayerPrefs.HasKey("muzik"))
                muzikDeger = PlayerPrefs.GetFloat("muzik");
            if (PlayerPrefs.HasKey("fare"))
                fareHassasiyet = PlayerPrefs.GetFloat("fare");

            GameManager.oyunDenetleyici.SesDegistir(sesDeger);
            GameManager.oyunDenetleyici.MuzikDegistir(muzikDeger);
            GameManager.oyunDenetleyici.FareHassasiyetDegistir(fareHassasiyet);
        }

        public void OyunuKapat()
        {
            Application.Quit();
        }
    }

}
