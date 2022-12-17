using SistemHatasi.Denetleyici;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SistemHatasi.OyuncuArayuzu
{
    public class OyunIciMenu : MenuDenetleyici
    {
        public static OyunIciMenu oyunMenu;

        [SerializeField] private Image _imlec;

        [SerializeField] private TMPro.TMP_Text etkilesimYazi;

        private void Awake()
        {
            if (oyunMenu)
                Destroy(this.gameObject);
            else
                oyunMenu = this;
        }

        public void DevamEt()
        {
            GameManager.oyunDenetleyici.DevamEt();
        }

        public void AnaMenuyeDon()
        {
            GameManager.oyunDenetleyici.AnaMenuyeDon();
        }

        public void ImleciKaldir()
        {
            _imlec.enabled = false;
        }

        public void ImleciAc()
        {
            _imlec.enabled = true;
        }

        public void EtkilesimeGir(string nesne)
        {
            etkilesimYazi.text = $"Etkilesime Gir : {nesne}";
        }

        public void EtkilesimdenCik()
        {
            etkilesimYazi.text = "";
        }
    }

}
