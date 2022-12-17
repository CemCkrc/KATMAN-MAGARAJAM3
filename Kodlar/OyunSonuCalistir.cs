using SistemHatasi.Arayuz;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SistemHatasi.Bilgisayar
{
    public class OyunSonuCalistir : MonoBehaviour, IEtkilesim
    {
        public void Etkilesim()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(3);
        }

        public string EtkilesimAdi()
        {
            return "Hata Sonlandirici";
        }
    }
}
