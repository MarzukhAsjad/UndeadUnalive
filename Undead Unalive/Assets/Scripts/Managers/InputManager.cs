using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class InputManager : MonoBehaviourSingleton<InputManager>
{
    private bool _disabledAxisInput = false;
    private bool _disabledJump = false;
    
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
        if(!_disabledAxisInput)
        {
            InputMouseXAxis = Input.GetAxis("Mouse X");
            InputMouseYAxis = Input.GetAxis("Mouse Y");
            InputXAxisRaw = Input.GetAxisRaw("Horizontal");
            InputYAxisRaw = Input.GetAxisRaw("Vertical");
        }

        if (!_disabledJump)
        {
            InputJump = Input.GetButtonDown("Jump");
        }
    }

    public void DisabledAxisInput()
    {
        _disabledAxisInput = true;
        InputMouseXAxis = InputMouseYAxis = InputXAxisRaw = InputYAxisRaw = 0;
    }
    
    public void DisabledJumpInput()
    {
        _disabledJump = true;
        InputJump = false;
    }
    
    public void DisabledAllUserInput()
    {
        DisabledAxisInput();
        DisabledJumpInput();
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
