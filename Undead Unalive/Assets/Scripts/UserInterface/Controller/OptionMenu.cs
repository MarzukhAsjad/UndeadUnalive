using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMenu : MonoBehaviour
{
    public GameObject MainMenu;

    public void Back ()
    {
        MainMenu.SetActive(true);
        this.gameObject.SetActive(false);
    } 
}
