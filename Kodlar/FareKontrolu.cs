using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SistemHatasi.Fare
{
    public class FareKontrolu : MonoBehaviour
    {
        public static void FareyiDegistir(bool gorunur, bool sabit)
        {
            Cursor.visible = gorunur;

            if (sabit)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }
    }
}
