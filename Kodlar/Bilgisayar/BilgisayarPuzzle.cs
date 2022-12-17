using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SistemHatasi.Bilgisayar
{
    [RequireComponent(typeof(AudioSource))]
    public class BilgisayarPuzzle : MonoBehaviour
    {
        private AudioSource _sesKaynak;

        public AudioClip kareIlerleme, baglantiYapildi;

        [SerializeField] BilgisayarArayuz _arayuz;

        [SerializeField] PuzzleParca[] _parcalar;

        [SerializeField] PuzzleParca[,] _parcalarMatris;

        public int puzzleX, puzzleY;

        public bool _aktif = false;

        public Color sifirlamaRenk, seciliNoktaRenk, silinenNokta, oyuncuRenk;

        private Vector2Int _seciliNokta;

        private Vector2Int _oyuncuPozisyon = Vector2Int.zero;

        private void Awake()
        {
            _sesKaynak = GetComponent<AudioSource>();

            _parcalarMatris = new PuzzleParca[puzzleX, puzzleY];

            int indeks = 0;

            for (int x = 0; x < puzzleX; x++)
            {
                for (int y = 0; y < puzzleY; y++)
                {
                    _parcalarMatris[x, y] = _parcalar[indeks];
                    indeks++;
                }
            }
        }

        private void Update()
        {
            if (!_aktif)
                return;

            if (Input.GetKeyDown(KeyCode.R))
            {
                PuzzleOlustur();
                return;
            }

            if (Input.GetButtonDown("Vertical"))
            {
                _aktif = false;
                float yonGetirX = Input.GetAxisRaw("Vertical");

                if (yonGetirX < 0f)
                {
                    SagaIlerle();
                }
                else if (yonGetirX > 0f)
                {
                    SolaIlerle();
                }

            }
            else if (Input.GetButtonDown("Horizontal"))
            {
                _aktif = false;
                float yonGetirY = Input.GetAxisRaw("Horizontal");

                if (yonGetirY < 0f)
                {
                    AsagiIlerle();
                }
                else if (yonGetirY > 0f)
                {
                    YukariIlerle();
                }
            }
        }

        public void PuzzleOlustur()
        {
            _seciliNokta.x = -1;
            _seciliNokta.y = -1;


            foreach (PuzzleParca item in _parcalar)
            {
                item.Sifirla(sifirlamaRenk);
            }

            for (int indeks = 0; indeks < 4; indeks++)
            {
                int xSil = Random.Range(0, puzzleX);
                int ySil = Random.Range(0, puzzleY);

                _parcalarMatris[xSil, ySil].silindi = true;
                _parcalarMatris[xSil, ySil].RenkDegis(silinenNokta);
            }

            _seciliNokta = SeciliNoktaGetir();
            _parcalarMatris[_seciliNokta.x, _seciliNokta.y].RenkDegis(seciliNoktaRenk);


            _oyuncuPozisyon = SeciliNoktaGetir();
            _parcalarMatris[_oyuncuPozisyon.x, _oyuncuPozisyon.y].RenkDegis(oyuncuRenk);

            _aktif = true;
        }

        private Vector2Int SeciliNoktaGetir()
        {
            int xNokta = Random.Range(0, puzzleX);
            int yNokta = Random.Range(0, puzzleY);

            if (_parcalarMatris[xNokta, yNokta].silindi)
                return SeciliNoktaGetir();
            else if (_seciliNokta.x == xNokta && _seciliNokta.y == yNokta)
                return SeciliNoktaGetir();
            else
                return new Vector2Int(xNokta, yNokta);
        }

        private void SolaIlerle()
        {
            if (_oyuncuPozisyon.x > 0)
            {
                if (!_parcalarMatris[_oyuncuPozisyon.x - 1, _oyuncuPozisyon.y].uzerindenGecildi &&
                   !_parcalarMatris[_oyuncuPozisyon.x - 1, _oyuncuPozisyon.y].silindi)
                {
                    _oyuncuPozisyon.x--;
                    _parcalarMatris[_oyuncuPozisyon.x, _oyuncuPozisyon.y].RenkDegis(oyuncuRenk);
                    _parcalarMatris[_oyuncuPozisyon.x, _oyuncuPozisyon.y].uzerindenGecildi = true;

                    _sesKaynak.PlayOneShot(kareIlerleme);

                    if (!HareketKontrol())
                        Invoke("SolaIlerle", 0.4f);
                }
                else
                {
                    _aktif = true;
                }
            }
            else
            {
                _aktif = true;
            }
        }

        private void SagaIlerle()
        {
            if (_oyuncuPozisyon.x < puzzleX - 1)
            {
                if (!_parcalarMatris[_oyuncuPozisyon.x + 1, _oyuncuPozisyon.y].uzerindenGecildi &&
                   !_parcalarMatris[_oyuncuPozisyon.x + 1, _oyuncuPozisyon.y].silindi)
                {
                    _oyuncuPozisyon.x++;
                    _parcalarMatris[_oyuncuPozisyon.x, _oyuncuPozisyon.y].RenkDegis(oyuncuRenk);
                    _parcalarMatris[_oyuncuPozisyon.x, _oyuncuPozisyon.y].uzerindenGecildi = true;
                    _sesKaynak.PlayOneShot(kareIlerleme);

                    if (!HareketKontrol())
                        Invoke("SagaIlerle", 0.4f);
                }
                else
                {
                    _aktif = true;
                }
            }
            else
            {
                _aktif = true;
            }
        }

        private void YukariIlerle()
        {
            if (_oyuncuPozisyon.y < puzzleY - 1)
            {
                if (!_parcalarMatris[_oyuncuPozisyon.x, _oyuncuPozisyon.y + 1].uzerindenGecildi &&
                   !_parcalarMatris[_oyuncuPozisyon.x, _oyuncuPozisyon.y + 1].silindi)
                {
                    _oyuncuPozisyon.y++;
                    _parcalarMatris[_oyuncuPozisyon.x, _oyuncuPozisyon.y].RenkDegis(oyuncuRenk);
                    _parcalarMatris[_oyuncuPozisyon.x, _oyuncuPozisyon.y].uzerindenGecildi = true;

                    _sesKaynak.PlayOneShot(kareIlerleme);

                    if (!HareketKontrol())
                        Invoke("YukariIlerle", 0.4f);
                }
                else
                {
                    _aktif = true;
                }
            }
            else
            {
                _aktif = true;
            }
        }

        private void AsagiIlerle()
        {
            if (_oyuncuPozisyon.y > 0)
            {
                if (!_parcalarMatris[_oyuncuPozisyon.x, _oyuncuPozisyon.y - 1].uzerindenGecildi &&
                   !_parcalarMatris[_oyuncuPozisyon.x, _oyuncuPozisyon.y - 1].silindi)
                {
                    _oyuncuPozisyon.y--;
                    _parcalarMatris[_oyuncuPozisyon.x, _oyuncuPozisyon.y].RenkDegis(oyuncuRenk);
                    _parcalarMatris[_oyuncuPozisyon.x, _oyuncuPozisyon.y].uzerindenGecildi = true;

                    _sesKaynak.PlayOneShot(kareIlerleme);

                    if (!HareketKontrol())
                        Invoke("AsagiIlerle", 0.4f);
                }
                else
                {
                    _aktif = true;
                }
            }
            else
            {
                _aktif = true;
            }
        }

        private bool HareketKontrol()
        {
            if (_oyuncuPozisyon.x == _seciliNokta.x && _oyuncuPozisyon.y == _seciliNokta.y)
            {
                _aktif = false;

                _sesKaynak.PlayOneShot(baglantiYapildi);

                Invoke("Baglandi", 1f);

                return true;
            }
            else
            {
                return false;
            }
        }

        private void Baglandi() => _arayuz.BaglantiDogrulandi();
    }
}