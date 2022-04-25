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
    #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
        }

        public void ActivateOptionMenu ()
        {
            OptionMenu.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
