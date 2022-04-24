using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoringSystem : MonoBehaviour
{
    public Text vaccineDisplay;
    public static int vaccineCount = 5;
    public Text maskDisplay;
    public static int maskCount = 5;
    public Text grenadeDisplay;
    public static int grenadeCount = 5;


    // Update is called once per frame
    void Update()
    {
        vaccineDisplay.text = "" + vaccineCount;
        maskDisplay.text = "" + maskCount;
        grenadeDisplay.text = "" + grenadeCount;
    }
}
