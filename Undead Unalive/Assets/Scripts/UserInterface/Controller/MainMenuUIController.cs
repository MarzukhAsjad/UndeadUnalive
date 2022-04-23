using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Interface.Controller
{
    
    public class MainMenuUIController : MonoBehaviour
    {
        public GameObject OptionMenu;
        public void PlayGame ()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }

        public void ActivateOptionMenu ()
        {
            OptionMenu.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
