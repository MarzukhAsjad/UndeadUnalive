using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class InputManager : MonoBehaviourSingleton<InputManager>
{
    private bool _disabledAxisInput = false;
    private bool _disabledJump = false;
    private bool _disabledKeyInput = false;
    
    public float InputMouseXAxis { get; private set; }
    public float InputMouseYAxis { get; private set; }
    public float InputXAxisRaw { get; private set; }
    public float InputYAxisRaw { get; private set; }
    public bool InputJump { get; private set; }
    
    public bool InputSprint { get; private set; }

    public bool KeyInteract { get; private set; }
    public bool KeyDash { get; private set; }
    public Vector2 MousePosition { get; private set; }
    
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
            MousePosition = Input.mousePosition;
        }

        if (!_disabledJump)
        {
            InputJump = Input.GetButtonDown("Jump");
        }

        if (!_disabledKeyInput)
        {
            KeyInteract = Input.GetKeyDown(KeyCode.E);
            KeyDash = Input.GetKeyDown(KeyCode.LeftControl);
            InputSprint = Input.GetKey(KeyCode.LeftShift);
        }
    }

    public void DisableAxisInput()
    {
        _disabledAxisInput = true;
        InputMouseXAxis = InputMouseYAxis = InputXAxisRaw = InputYAxisRaw = 0;
    }
    
    public void DisableJumpInput()
    {
        _disabledJump = true;
        InputJump = false;
    }
    
    public void DisableKeyInput()
    {
        _disabledKeyInput = true;
        KeyInteract = false;
    }
    
    public void EnableAxisInput()
    {
        _disabledAxisInput = false;
    }
    
    public void EnableJumpInput()
    {
        _disabledJump = false;
    }
    
    public void EnableKeyInput()
    {
        _disabledKeyInput = false;
    }
    
    public void DisableAllUserInput()
    {
        DisableAxisInput();
        DisableJumpInput();
        DisableKeyInput();
    }
    
    public void EnableAllUserInput()
    {
        EnableAxisInput();
        EnableJumpInput();
        EnableKeyInput();
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
