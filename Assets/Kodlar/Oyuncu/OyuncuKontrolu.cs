using SistemHatasi.Arayuz;
using SistemHatasi.Denetleyici;
using SistemHatasi.OyuncuArayuzu;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SistemHatasi.Oyuncu
{
    [RequireComponent(typeof(CharacterController), typeof(Animator))]
    public class OyuncuKontrolu : MonoBehaviour
    {
        [SerializeField] private Transform _kameraPozisyon;
        [SerializeField] private Transform _yerKontrolPozisyon;

        private OyuncuAyakSesi _oyuncuSes;
        private CharacterController _kontrolcu;
        private Animator _oyuncuAnimator;

        private OyuncuHareketTipleri _oyuncuHareketTipi;
        public LayerMask yerKontrolKatman;

        private float _fareDikey = 0f;

        public float yurumeHizi;
        public float kosmaHizi;

        private float _yerCekimi = -9.8f;

        #region EtkilesimDegerler

        public float etkilesimMesafe = 4f;
        public LayerMask etkilesimKatman;
        private RaycastHit _carpisma;

        #endregion


        private void Awake()
        {
            _oyuncuAnimator = this.GetComponent<Animator>();
            _kontrolcu = this.GetComponent<CharacterController>();

            _oyuncuSes = this.GetComponentInChildren<OyuncuAyakSesi>();
        }

        private void OnEnable()
        {
            _oyuncuAnimator.enabled = true;
            _kontrolcu.enabled = true;
        }

        private void OnDisable()
        {
            _oyuncuAnimator.enabled = false;
            _kontrolcu.enabled = false;
            OyunIciMenu.oyunMenu.EtkilesimdenCik();
        }

        private void Update()
        {
            if (GameManager.oyunDenetleyici.oyunDuraklatildi)
                return;

            FareHareket();
            Hareket();
            Etkilesim();

            AnimasyonCalistir();

            MenuKontrol();
        }

        private void MenuKontrol()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                GameManager.oyunDenetleyici.Duraklat();
                OyunIciMenu.oyunMenu.MenuAc(0);
            }
        }

        private void FareHareket()
        {
            float xHareket = Input.GetAxis("Mouse X") * GameManager.oyunDenetleyici.fareHassasiyet;
            float yHareket = Input.GetAxis("Mouse Y") * GameManager.oyunDenetleyici.fareHassasiyet;

            _fareDikey -= yHareket * Time.deltaTime;

            _fareDikey = Mathf.Clamp(_fareDikey, -70f, 70f);

            transform.eulerAngles += new Vector3(0f, xHareket, 0f) * Time.deltaTime;

            _kameraPozisyon.transform.localEulerAngles = new Vector3(_fareDikey, 0f, 0f);
        }

        private void Hareket()
        {
            bool hareketEdebilir = yerdeMi();

            float xHareket = Input.GetAxis("Horizontal");
            float yHareket = Input.GetAxis("Vertical");

            bool kosuyor = Input.GetButton("Kosma");

            Vector3 hareketYon = new Vector3(xHareket, 0f, yHareket);

            hareketYon = transform.TransformDirection(hareketYon);

            if (kosuyor && hareketYon.magnitude > 0.1f)
            {
                _oyuncuHareketTipi = OyuncuHareketTipleri.Kosuyor;
                hareketYon *= kosmaHizi;
            }
            else if (hareketYon.magnitude > 0.1f)
            {
                _oyuncuHareketTipi = OyuncuHareketTipleri.Yuruyor;
                hareketYon *= yurumeHizi;
            }
            else
                _oyuncuHareketTipi = OyuncuHareketTipleri.Hareketsiz;

            if (hareketEdebilir)
            {
                hareketYon = new Vector3(hareketYon.x, _yerCekimi, hareketYon.z);
                _kontrolcu.Move(hareketYon * Time.deltaTime);
            }
        }

        private void Etkilesim()
        {
            if(Physics.Raycast(_kameraPozisyon.transform.position, _kameraPozisyon.transform.forward, out _carpisma, etkilesimMesafe, etkilesimKatman))
            {
                IEtkilesim etkilesimObje = _carpisma.transform.GetComponent<IEtkilesim>();

                if(etkilesimObje != null) 
                    OyunIciMenu.oyunMenu.EtkilesimeGir(etkilesimObje.EtkilesimAdi());

                if (Input.GetMouseButtonDown(0))
                {
                    if (etkilesimObje != null)
                    {
                        etkilesimObje.Etkilesim();
                    }
                    else
                    {
                        OyunIciMenu.oyunMenu.EtkilesimdenCik();
                    }
                }
            }
            else
            {
                OyunIciMenu.oyunMenu.EtkilesimdenCik();
            }
        }

        private void AnimasyonCalistir()
        {
            _oyuncuSes.SesCalistir(_oyuncuHareketTipi);

            switch (_oyuncuHareketTipi)
            {
                case OyuncuHareketTipleri.Hareketsiz:
                    _oyuncuAnimator.SetFloat("Hareket", 0f);
                    break;
                case OyuncuHareketTipleri.Yuruyor:
                    _oyuncuAnimator.SetFloat("Hareket", 1f);
                    break;
                case OyuncuHareketTipleri.Kosuyor:
                    _oyuncuAnimator.SetFloat("Hareket", 2f);
                    break;
                default:
                    break;
            }
        }

        private bool yerdeMi()
        {
            return Physics.CheckSphere(_yerKontrolPozisyon.position, 1f, yerKontrolKatman);
        }
    }
}

public enum OyuncuHareketTipleri
{
    Hareketsiz,
    Yuruyor,
    Kosuyor
}