// #define DEBUG_GROUND

using Characters.Entity;
using Interface.Player;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /*
     *
     * In editor config
     * 
     */
    public string cameraName = "Main Camera";
    public float movementSpeed = 4;
    public float maxMovementSpeed = 5;
    public float sprintMultiplier = 1.5f;
    public float jumpHeight = 1.5f;
    public float gravity = 35;
    public LayerMask groundMask;
    public bool forceCameraOnPlayer = false;
    public float cameraSwayDistance = 0.03f;
    public float cameraBobPeriod = 0.4f;

    /*
     *
     * Properties
     * 
     */
    private Camera _mainCamera;
    private float _cameraDefaultHeight;

    private CharacterController _characterController;
    private float _playerHeight;
    private float _playerRadius;

    private Vector3 _playerVelocity;

    private bool _isOnGround;
    private float _movementControlRatio = 1;

    private bool _isMoving;
    private bool _isCameraAnimating;
    private float _cameraAnimationTimer;

    private PlayerInteractable _PlayerInteractableRaycastResult;
    // private GameObject _raycastObject;

    // Start is called before the first frame update
    private void Start()
    {
        InputManager.LockMouse();
        _characterController = GetComponent<CharacterController>();
        _playerHeight = _characterController.height;
        _playerRadius = _characterController.radius;

        if (forceCameraOnPlayer)
        {
            _mainCamera = GameObject.Find(cameraName).GetComponent<Camera>();
            if (_mainCamera.transform.parent != transform) _mainCamera.transform.parent = transform;
            _cameraDefaultHeight = _playerHeight / 2 - 0.25f;
            _mainCamera.transform.localPosition = new Vector3(0, _cameraDefaultHeight, 0);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        UpdatePlayerAction();
        UpdateCameraView();
        UpdateRaycast();
        KeyInputHandler();
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
        var isLastFrameOnGround = _isOnGround;
        _isOnGround = Physics.CheckBox(_characterController.transform.position - new Vector3(0, _playerHeight / 2 + 0.2f, 0), new Vector3(_playerRadius, 0.2f, _playerRadius), Quaternion.identity, groundMask);

        // fall damage
        if (!isLastFrameOnGround && _isOnGround)
        {
            var diff = _playerVelocity.y + gravity / 2;

            if (diff < 0)
            {
                GetComponent<CharacterEntity>().AddDeltaHealth(diff);
            }
        }

        #if DEBUG_GROUND
        var collisions = Physics.OverlapSphere(_characterController.transform.position - new Vector3(0, _playerHeight / 2, 0), 1, groundMask);
        #endif

        _movementControlRatio = _isOnGround ? 1f : 0.0015f;

        if (_isOnGround && _playerVelocity.y < 0)
        {
            // "reset" gravity
            _playerVelocity.y = -2;

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
    private void UpdateCameraView()
    {
        if (!forceCameraOnPlayer) return;

        var newAngle = _mainCamera.transform.localRotation.eulerAngles.x - InputManager.Instance.InputMouseYAxis;

        // angle "below" zero will reset to 360, undo it
        newAngle = newAngle > 180 ? newAngle - 360 : newAngle;

        _mainCamera.transform.localRotation = Quaternion.Euler(Mathf.Clamp(newAngle, -90, 90), 0, 0);
        transform.Rotate(Vector3.up * InputManager.Instance.InputMouseXAxis);

        if (_isCameraAnimating)
        {
            var deltaTime = Time.deltaTime;
            if (_isOnGround && InputManager.Instance.InputSprint)
                deltaTime *= sprintMultiplier;

            _cameraAnimationTimer += deltaTime;

            var ratio = Mathf.PI / cameraBobPeriod;

            var sinWalkTime = Mathf.Sin((_cameraAnimationTimer - cameraBobPeriod / 4) * 2 * ratio);
            sinWalkTime = (sinWalkTime + 1) / 2;
            var sin2WalkTime = Mathf.Sin(_cameraAnimationTimer * ratio);

            var mainCameraTransform = _mainCamera.transform;
            var temPos = mainCameraTransform.localPosition;
            temPos.x = sin2WalkTime * cameraSwayDistance * 2;
            temPos.y = _cameraDefaultHeight + sinWalkTime * cameraSwayDistance;
            mainCameraTransform.localPosition = temPos;
        }
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
        unitMovement += temTransform.right * InputManager.Instance.InputXAxisRaw;
        unitMovement += temTransform.forward * InputManager.Instance.InputYAxisRaw;
        unitMovement = unitMovement.normalized; // no boost for diag

        _isMoving = unitMovement.magnitude > float.Epsilon;
        _isCameraAnimating = _isMoving && _isOnGround;

        var verticalVelocity = _playerVelocity.y; // backup
        _playerVelocity *= _isOnGround ? 0.1f : 0.999f; // friction
        _playerVelocity += movementSpeed * unitMovement * _movementControlRatio; // player control

        var maxSpeed = maxMovementSpeed;
        if (_isOnGround && InputManager.Instance.InputSprint)
            maxSpeed *= sprintMultiplier;

        // cap max speed
        _playerVelocity.y = 0;
        if (_playerVelocity.magnitude > maxSpeed)
            _playerVelocity = _playerVelocity.normalized * maxSpeed;
        
        _playerVelocity.y = verticalVelocity; // restore

        /*
         *
         * Jump
         * 
         */
        if (_isOnGround && InputManager.Instance.InputJump)
        {
            _playerVelocity.y = Mathf.Sqrt(jumpHeight * gravity * 2);
        }

        /*
         *
         * Velocity
         * 
         */
        _characterController.Move(Time.deltaTime * _playerVelocity);
    }

    void UpdateRaycast()
    {
        var hits = new RaycastHit[10];
        var size = Physics.RaycastNonAlloc(_mainCamera.ScreenPointToRay(InputManager.Instance.MousePosition), hits);
        for (var i = 0; i < size; ++i)
            if (hits[i].transform.gameObject.TryGetComponent(out _PlayerInteractableRaycastResult))
                return;

        _PlayerInteractableRaycastResult = null;
    }


    void KeyInputHandler()
    {
        if (InputManager.Instance.KeyInteract)
        {
            if (_PlayerInteractableRaycastResult is not null)
            {
                _PlayerInteractableRaycastResult.OnInteract(gameObject);
            }
        }
    }
}
