using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SistemHatasi.Bilgisayar
{
    public class KodYerlestirici : MonoBehaviour
    {
        public static KodYerlestirici yerlestirici;

        public KodBlokObjeKontrol kodKatmanBlok;

        private void Awake()
        {
            if (yerlestirici)
                Destroy(this.gameObject);
            else
                yerlestirici = this;
        }

        public void KodlariYerlestir(Kod kod, int katman)
        {
            kodKatmanBlok.NumaraYerlestir(kod, katman);
        }
    }

    public class Kod
    {
        public int[] kodDeger;

        public Kod()
        {
            kodDeger = new int[4];
        }
    }
}
