using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SistemHatasi.Bilgisayar
{
    public class KodBlokObjeKontrol : MonoBehaviour
    {
        public Material[] kodBloklayiciMateryaller; //KIRMIZI, YESIL, SARI, MAVI, BEYAZ

        public GameObject kodBloklayici; //Bloklayici

        public GameObject[] kodObjeleri; //0-9 Obje

        public Transform[] kodPozisyonlari;

        public int katSayisi = 0, kodKonumSayisi = 0;

        private Transform[,] _katmanliKodPozisyonlari;

        private Transform[,] _katmanliKodBlokObjeleri;

        private KABLOLAR[,] _blokluObjeler;

        private void Awake()
        {
            _katmanliKodPozisyonlari = new Transform[katSayisi, kodKonumSayisi];
            _katmanliKodBlokObjeleri = new Transform[katSayisi, kodKonumSayisi];
            _blokluObjeler = new KABLOLAR[katSayisi, kodKonumSayisi];

            int indeks = 0;

            for (int x = 0; x < katSayisi; x++)
            {
                for (int y = 0; y < kodKonumSayisi; y++)
                {
                    _katmanliKodPozisyonlari[x, y] = kodPozisyonlari[indeks];

                    _katmanliKodBlokObjeleri[x, y] = Instantiate(kodBloklayici, _katmanliKodPozisyonlari[x, y].position, _katmanliKodPozisyonlari[x, y].rotation, null).GetComponent<Transform>();
                    _katmanliKodBlokObjeleri[x, y].gameObject.name = $"Bloklayici {x},{y}"; 
                    RenkAta(x, y);
                    MateryalAta(x, y);

                    indeks++;
                }
            }
        }

        public void NumaraYerlestir(Kod kod, int katman)
        {
            for (int indeks = 0; indeks < 4; indeks++)
            {
                Instantiate(kodObjeleri[kod.kodDeger[indeks]], _katmanliKodPozisyonlari[katman, indeks].position, _katmanliKodPozisyonlari[katman, indeks].rotation, null);
            }
        }

        public void ObjeleriBlokla(KABLOLAR renk, int katman)
        {
            for (int indeks = 0; indeks < kodKonumSayisi; indeks++)
            {
                if (_blokluObjeler[katman, indeks] == renk)
                {
                    _katmanliKodBlokObjeleri[katman, indeks].gameObject.SetActive(false);
                }
            }
        }

        public void ObjeBlokKaldir(KABLOLAR renk, int katman)
        {
            for (int indeks = 0; indeks < kodKonumSayisi; indeks++)
            {
                if (_blokluObjeler[katman, indeks] == renk)
                {
                    _katmanliKodBlokObjeleri[katman, indeks].gameObject.SetActive(true);
                }
            }
        }

        private void MateryalAta(int x, int y)
        {
            _katmanliKodBlokObjeleri[x, y].GetComponent<MeshRenderer>().material = kodBloklayiciMateryaller[(int)_blokluObjeler[x, y]];
        }

        private void RenkAta(int x, int y)
        {
            int kabloRenkNumara = Random.Range(0, 5);

            switch (kabloRenkNumara)
            {
                case 0:
                    _blokluObjeler[x, y] = KABLOLAR.KIRMIZI;
                    break;
                case 1:
                    _blokluObjeler[x, y] = KABLOLAR.YESIL;
                    break;
                case 2:
                    _blokluObjeler[x, y] = KABLOLAR.SARI;
                    break;
                case 3:
                    _blokluObjeler[x, y] = KABLOLAR.MAVI;
                    break;
                case 4:
                    _blokluObjeler[x, y] = KABLOLAR.BEYAZ;
                    break;
                default:
                    _blokluObjeler[x, y] = KABLOLAR.KIRMIZI;
                    break;
            }
        }

        public void RenkKatmanlariniCikisAygitinaYukle(bool[] cikisAygit, int katman)
        {
            for (int indeks = 0; indeks < cikisAygit.Length; indeks++)
            {
                cikisAygit[indeks] = false;
            }

            for (int indeks = 0; indeks < kodKonumSayisi; indeks++)
            {
                Debug.Log(katman);
                switch (_blokluObjeler[katman, indeks])
                {
                    case KABLOLAR.KIRMIZI:
                        cikisAygit[0] = true;
                        break;
                    case KABLOLAR.YESIL:
                        cikisAygit[1] = true;
                        break;
                    case KABLOLAR.SARI:
                        cikisAygit[2] = true;
                        break;
                    case KABLOLAR.MAVI:
                        cikisAygit[3] = true;
                        break;
                    case KABLOLAR.BEYAZ:
                        cikisAygit[4] = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
