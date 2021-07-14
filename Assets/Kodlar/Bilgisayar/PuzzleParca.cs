using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SistemHatasi.Bilgisayar
{
    public class PuzzleParca : MonoBehaviour
    {
        UnityEngine.UI.Image resim;

        public bool silindi = false;

        public bool uzerindenGecildi = false;

        private void Awake()
        {
            resim = GetComponent<UnityEngine.UI.Image>();
        }

        public void RenkDegis(Color yeniRenk) => resim.color = yeniRenk;

        public void Sifirla(Color sifirRenk)
        {
            resim.color = sifirRenk;
            uzerindenGecildi = false;
            silindi = false;
        }
    }
}
