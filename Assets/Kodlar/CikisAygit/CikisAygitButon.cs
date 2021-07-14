using SistemHatasi.Arayuz;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SistemHatasi.CikisAygiti
{
    public class CikisAygitButon : MonoBehaviour, IEtkilesim
    {
        public int kabloNumara = 0;

        public void Etkilesim() => GetComponentInParent<CikisAygiti>().KabloDegistir(kabloNumara);

        public string EtkilesimAdi()
        {
            string sonuc = "";

            switch (kabloNumara)
            {
                case 0:
                    sonuc = "KIRMIZI BUTON";
                    break;
                case 1:
                    sonuc = "YESIL BUTON";
                    break;
                case 2:
                    sonuc = "SARI BUTON";
                    break;
                case 3:
                    sonuc = "MAVI BUTON";
                    break;
                case 4:
                    sonuc = "BEYAZ BUTON";
                    break;
                default:
                    sonuc = "CIKIS AYGITI";
                    break;
            }
            return sonuc;
        }
    }
}
