using SistemHatasi.Arayuz;
using SistemHatasi.Oyuncu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SistemHatasi.Bilgisayar
{
    public class BilgisayarKontrolu : MonoBehaviour, IEtkilesim
    {
        private OyuncuKontrolu _oyuncu;
        private Transform _oyuncuKamera;

        private BilgisayarArayuz arayuz;

        private bool _calisiyor = false;
        private bool _cikisYapiliyor = false;
        private bool _girisYapiliyor = false;

        private float _hareketOrani = 0f;
        private float _adimOrani = 0f;

        [SerializeField] private Transform _pozisyon;

        [SerializeField] private Transform _oyuncuEtkilesimOncesiPozisyon;

        private void Awake() => arayuz = GetComponent<BilgisayarArayuz>();

        private void Start()
        {
            _oyuncu = GameObject.FindObjectOfType<OyuncuKontrolu>();
            _oyuncuKamera = _oyuncu.transform.GetChild(0).GetChild(0);
        }

        public void Etkilesim()
        {
            _oyuncuEtkilesimOncesiPozisyon.position = _oyuncuKamera.transform.position;
            _oyuncuEtkilesimOncesiPozisyon.rotation = _oyuncuKamera.transform.rotation;

            _oyuncu.enabled = false;

            _girisYapiliyor = true;
        }

        void Update()
        {
            if (_girisYapiliyor)
            {
                _adimOrani += Time.deltaTime * 0.1f;
                _hareketOrani = Mathf.MoveTowards(_hareketOrani, 1f, _adimOrani);

                if (Tasi(_oyuncuKamera.transform, _pozisyon, _hareketOrani))
                {
                    arayuz.Calistir();

                    _hareketOrani = 0f;
                    _adimOrani = 0f;
                    _girisYapiliyor = false;
                    _calisiyor = true;
                    return;
                }
            }

            if (!_calisiyor)
                return;

            if (_cikisYapiliyor)
            {
                arayuz.CikisYap();

                _adimOrani += Time.deltaTime * 0.1f;
                _hareketOrani = Mathf.MoveTowards(_hareketOrani, 1f, _adimOrani);

                if (Tasi(_oyuncuKamera.transform, _oyuncuEtkilesimOncesiPozisyon, _hareketOrani))
                {
                    _adimOrani = 0f;
                    _hareketOrani = 0f;
                    CikisYap();
                    return;
                }
            }


            if (Input.GetMouseButtonDown(1))
            {
                _cikisYapiliyor = true;
                return;
            }
        }

        private bool Tasi(Transform ilkPozisyon, Transform ikinciPozisyon, float sure)
        {
            ilkPozisyon.position = Vector3.Lerp(ilkPozisyon.position, ikinciPozisyon.position, sure);
            ilkPozisyon.rotation = Quaternion.Lerp(ilkPozisyon.rotation, ikinciPozisyon.rotation, sure);

            if (sure >= 1f)
                return true;
            else
                return false;
        }

        private void CikisYap()
        {
            _calisiyor = false;
            _cikisYapiliyor = false;

            _oyuncu.enabled = true;
        }

        public string EtkilesimAdi() => "BILGISAYAR";
    }
}