using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    [SerializeField] private AnimationCurve lightCurve;
    private float _timer;

    [SerializeField] private Light targetLight;
    private float defaultIntensity;

    private void Start()
    {
        defaultIntensity = targetLight.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime / 3;

        targetLight.intensity = lightCurve.Evaluate(_timer % 1.0f) * defaultIntensity;
    }
}
