using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    // Start is called before the first frame update
    public Light mylight;
    public float timeDelay;
    public float timeInterval;
    public float maxIntensity;
    private AudioSource audio;
    void Start()
    {
        audio = GetComponent<AudioSource>();
        InvokeRepeating("StartLightning", 5.0f, timeInterval);
    }

    private void StartLightning()
    {
        StartCoroutine("IncreaseLighting");
    }
    IEnumerator IncreaseLighting()
    {
        mylight.intensity = maxIntensity;
        yield return new WaitForSeconds(timeDelay);
        mylight.intensity = 0.1f;
        audio.Play();
    }



}
