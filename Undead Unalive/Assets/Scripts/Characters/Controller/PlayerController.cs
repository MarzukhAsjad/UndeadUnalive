// #define DEBUG_GROUND

using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /*
     *
     * In editor config
     * 
     */
    public String cameraName = "Main Camera";
    public float movementSpeed = 5;
    public float maxMovementSpeed = 1;
    public float JumpHeight = 1;
    public float gravity = 9.81f;
    public LayerMask groundMask;

    /*
     *
     * Properties
     * 
     */
    private Camera _mainCamera;

    private CharacterController _characterController;
    private float _playerHeight;
    private float _playerRadius;

    private Vector3 _playerVelocity = new Vector3();

    private bool _isOnGround = false;
    private float _movementControlRatio = 1;

    // Start is called before the first frame update
    private void Start()
    {
        InputManager.LockMouse();
        _characterController = GetComponent<CharacterController>();
        _playerHeight = _characterController.height;
        _playerRadius = _characterController.radius;

        _mainCamera = GameObject.Find(cameraName).GetComponent<Camera>();
        _mainCamera.transform.localPosition = new Vector3(0, _playerHeight / 2 - 0.25f, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateViewAngle();
        UpdatePlayerAction();
    }

    #if DEBUG_GROUND
    private void OnDrawGizmos()
    {
        if(_characterController == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_characterController.transform.position - new Vector3(0, _playerHeight / 2, 0), 0.3f);
    }
    #endif

    private void FixedUpdate()
    {
        _playerVelocity.y -= (gravity * Time.deltaTime);
        _isOnGround = Physics.CheckBox(_characterController.transform.position - new Vector3(0, _playerHeight / 2 + 0.2f, 0), new Vector3(_playerRadius, 0.2f, _playerRadius), Quaternion.identity, groundMask);

        #if DEBUG_GROUND
        var collisions = Physics.OverlapSphere(_characterController.transform.position - new Vector3(0, _playerHeight / 2, 0), 1, groundMask);
        #endif

        _movementControlRatio = _isOnGround ? 1f : 0.0015f;

        if (_isOnGround && _playerVelocity.y < 0)
        {
            // "reset" gravity
            _playerVelocity.y = -1 / movementSpeed;

            #if DEBUG_GROUND
            foreach (var collision in collisions)
            {
                print(collision.gameObject.name);
            }
            #endif
        }
    }

    /*
     *
     * Event
     * 
     */
    private void UpdateViewAngle()
    {
        var newAngle = _mainCamera.transform.localRotation.eulerAngles.x - InputManager.Instance.InputMouseYAxis;

        // angle "below" zero will reset to 360, undo it
        newAngle = newAngle > 180 ? newAngle - 360 : newAngle;

        _mainCamera.transform.localRotation = Quaternion.Euler(Mathf.Clamp(newAngle, -90, 90), 0, 0);
        transform.Rotate(Vector3.up * InputManager.Instance.InputMouseXAxis);
    }

    private void UpdatePlayerAction()
    {
        var temTransform = transform;

        /*
         *
         * Move
         * 
         */
        var unitMovement = new Vector3();
        unitMovement += temTransform.right * InputManager.Instance.InputXAxis;
        unitMovement += temTransform.forward * InputManager.Instance.InputYAxis;
        unitMovement = unitMovement.normalized; // no boost for diag

        var verticalVelocity = _playerVelocity.y; // backup
        _playerVelocity *= _isOnGround ? 0.1f : 0.999f; // friction
        _playerVelocity += movementSpeed * unitMovement * _movementControlRatio; // player control

        // cap max speed
        _playerVelocity.y = 0;
        if (_playerVelocity.magnitude > maxMovementSpeed)
            _playerVelocity = _playerVelocity.normalized * maxMovementSpeed;

        _playerVelocity.y = verticalVelocity; // restore

        /*
         *
         * Jump
         * 
         */
        if (_isOnGround && InputManager.Instance.InputJump)
        {
            _playerVelocity.y = Mathf.Sqrt(JumpHeight * gravity * 2);
        }

        /*
         *
         * Velocity
         * 
         */
        _characterController.Move(Time.deltaTime * _playerVelocity);
    }
}
