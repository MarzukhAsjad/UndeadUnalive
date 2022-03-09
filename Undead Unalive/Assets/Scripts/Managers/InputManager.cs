using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class InputManager : MonoBehaviourSingleton<InputManager>
{
    public float InputMouseXAxis { get; private set; }
    public float InputMouseYAxis { get; private set; }
    public float InputXAxisRaw { get; private set; }
    public float InputYAxisRaw { get; private set; }
    public bool InputJump { get; private set; }
    
    // Start is called before the first frame update
    // void Start() { }

    // Update is called once per frame
    private void Update()
    {
        InputMouseXAxis = Input.GetAxis("Mouse X");
        InputMouseYAxis = Input.GetAxis("Mouse Y");
        InputXAxisRaw = Input.GetAxisRaw("Horizontal");
        InputYAxisRaw = Input.GetAxisRaw("Vertical");
        InputJump = Input.GetButtonDown("Jump");
    }

    public static void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public static void ReleaseMouse()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
