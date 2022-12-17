using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SistemHatasi.SahneGecis
{
    public class SahneGecisiYap : MonoBehaviour
    {
        public bool cikisYap = false;

        void Update()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                cikisYap = true;
            }

            if (cikisYap)
            {
                cikisYap = false;
                if (SceneManager.GetActiveScene().buildIndex == 1)
                    SceneManager.LoadScene(2);
                else
                    SceneManager.LoadScene(0);

                return;
            }
        }
    }
}
