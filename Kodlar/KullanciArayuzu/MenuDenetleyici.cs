using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SistemHatasi.Denetleyici
{
    public class MenuDenetleyici : MonoBehaviour
    {
        [SerializeField] private Menu[] _menuler;

        public int baslangicMenuNumara;

        private void Start()
        {
            MenuAc(baslangicMenuNumara);
        }

        public void MenuAc(int menuNumara)
        {
            if (menuNumara == -1)
                return;

            for (int indeks = 0; indeks < _menuler.Length; indeks++)
            {
                if (indeks == menuNumara)
                {
                    _menuler[indeks].transform.gameObject.SetActive(true);
                }
            }
        }
        public void MenuKapa(int menuNumara)
        {
            if (menuNumara == -1)
                return;

            for (int indeks = 0; indeks < _menuler.Length; indeks++)
            {
                if (indeks == menuNumara)
                {
                    _menuler[indeks].transform.gameObject.SetActive(false);
                }
            }
        }

    }
}
