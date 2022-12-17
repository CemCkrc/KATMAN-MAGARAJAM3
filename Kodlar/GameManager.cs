using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SistemHatasi.Denetleyici
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager oyunDenetleyici;

        [SerializeField] private AudioMixer _mixer;

        public float fareHassasiyet
        {
            get;
            private set;
        } = 100f;

        /*public int kalanDakika
        {
            get;
            private set;
        } = 10;

        public int gecenDakika
        {
            get;
            private set;
        } = 0;

        public float gecenSaniye
        {
            get;
            private set;
        } = 0f;
        */

        // private bool _zamanlamaBasladi = false;

        public bool oyunDuraklatildi
        {
            get;
            private set;
        } = false;

        private void Awake()
        {
            if (oyunDenetleyici)
            {
                Destroy(this.gameObject);
            }
            else
            {
                oyunDenetleyici = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }

        private void Start()
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

            SesDegistir(sesDeger);
            MuzikDegistir(muzikDeger);
            FareHassasiyetDegistir(fareHassasiyet);
        }
        /*
        private void Update()
        {
            if (!_zamanlamaBasladi)
                return;

            gecenSaniye += Time.deltaTime;

            if(gecenSaniye > 60f)
            {
                gecenSaniye = 0f;
                gecenDakika++;
                OyunSonuKontrol();
            }

            //Debug.Log($"Gecen Zaman {gecenDakika} : {gecenSaniye}");
        }
        private void OyunSonuKontrol()
        {
            if (gecenDakika > kalanDakika)
            {
                Debug.Log("Kaybedildi");
                //Kaybetme sahnesi yukle
            }
        }
        */

        private void OnLevelWasLoaded(int level)
        {
            if (level == 0)
            {
                Fare.FareKontrolu.FareyiDegistir(true, false);
                //GeriSayimBaslat();
            }
            else
            {
                Fare.FareKontrolu.FareyiDegistir(false, true);
            }
        }

        public void SesDegistir(float deger)
        {
            _mixer.SetFloat("SesVol", deger - 80f);
        }

        public void MuzikDegistir(float deger)
        {
            _mixer.SetFloat("MuzikVol", deger - 80f);
        }

        public void FareHassasiyetDegistir(float deger)
        {
            fareHassasiyet = deger;
        }
        /*
        private void GeriSayimBaslat()
        {
            _zamanlamaBasladi = true;
        }
        */

        public void Duraklat()
        {
            int indeks = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            if (indeks == 1 || indeks == 3)
                return;

            oyunDuraklatildi = true;
            Time.timeScale = 0f;
            Fare.FareKontrolu.FareyiDegistir(true, false);
        }

        public void DevamEt()
        {
            oyunDuraklatildi = false;
            Time.timeScale = 1f;
            Fare.FareKontrolu.FareyiDegistir(false, true);
        }

        public void AnaMenuyeDon()
        {
            oyunDuraklatildi = false;
            //_zamanlamaBasladi = false;
            //gecenDakika = 0;
            //gecenSaniye = 0f;

            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
