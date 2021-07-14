using SistemHatasi.Arayuz;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SistemHatasi.ParazitOnleyici
{
    public class ParazitOnleyiciButon : MonoBehaviour, IEtkilesim
    {
        public Yonler yon;

        public void Etkilesim()
        {
            switch (yon)
            {
                case Yonler.YUKARI:
                    GetComponentInParent<ParazitOnleyici>().YukariTusBasma();
                    break;
                case Yonler.ASAGI:
                    GetComponentInParent<ParazitOnleyici>().AsagiTusBasma();
                    break;
                case Yonler.SOL:
                    GetComponentInParent<ParazitOnleyici>().SolTusBasma();
                    break;
                case Yonler.SAG:
                    GetComponentInParent<ParazitOnleyici>().SagTusBasma();
                    break;
                case Yonler.ORTA:
                    GetComponentInParent<ParazitOnleyici>().ParazitCozumKontrol();
                    break;
                default:
                    break;
            }
        }

        public string EtkilesimAdi()
        {
            string sonuc = "";

            switch (yon)
            {
                case Yonler.YUKARI:
                    sonuc = "YUKARI BUTON";
                    break;
                case Yonler.ASAGI:
                    sonuc = "ASAGI BUTON";
                    break;
                case Yonler.SOL:
                    sonuc = "SOL BUTON";
                    break;
                case Yonler.SAG:
                    sonuc = "SAG BUTON";
                    break;
                case Yonler.ORTA:
                    sonuc = "ORTA BUTON";
                    break;
                default:
                    sonuc = "PARAZIT ONLEYICI";
                    break;
            }
            return sonuc;
        }
    }
}