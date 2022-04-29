// #define DEBUG_GROUND

using Characters.Entity;
using Interface.Player;
using UnityEngine;
using Utilities;

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
    public float dashMultiplier = 100;
    public AudioClip jumpAudio;
    public AudioClip landAudio;

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

    private Vector3 _playerDefaultHandPosition;

    private PlayerInteractable _playerInteractableRaycastResult;
    private Vector3 _playerInteractableRaycastResultPosition;

    private bool _sprintPenalty;

    private AudioRandomizer _walkAudio;

    private CharacterEntity _characterEntity;
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

        _characterEntity = GetComponent<CharacterEntity>();
        _playerDefaultHandPosition = _mainCamera.transform.GetChild(0).localPosition;

        _walkAudio = GetComponent<AudioRandomizer>();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdatePlayerAction();
        UpdateCameraView();
        UpdateRaycast();
        KeyInputHandler();

        RechargeStamina();
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
        _isOnGround =
            Physics.CheckBox(_characterController.transform.position - new Vector3(0, _playerHeight / 2 + 0.2f, 0),
                new Vector3(_playerRadius, 0.2f, _playerRadius), Quaternion.identity, groundMask);

        // fall damage
        if (!isLastFrameOnGround && _isOnGround)
        {
            GetComponent<AudioSource>().PlayOneShot(landAudio);
            var diff = _playerVelocity.y + gravity / 2;

            if (diff < 0)
            {
                _characterEntity.AddDeltaHealth(diff);
            }
        }

#if DEBUG_GROUND
        var collisions =
 Physics.OverlapSphere(_characterController.transform.position - new Vector3(0, _playerHeight / 2, 0), 1, groundMask);
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

    private void RechargeStamina()
    {
        var staminaRecharge = Time.deltaTime * 10;
        var maxStamina = _characterEntity.GetDefaultMaxStamina();
        var currentStamina = _characterEntity.GetStamina();
        var newStamina = staminaRecharge + _characterEntity.GetStamina();
        if (currentStamina < maxStamina)
        {
            _characterEntity.ChangeStamina(Mathf.Min(newStamina, maxStamina));
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
                deltaTime *= Mathf.Min(sprintMultiplier, 1.5f);

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

            // hand movement
            temPos.x = (1 - sin2WalkTime) * cameraSwayDistance * 2;
            temPos.y = _cameraDefaultHeight + (1 - sinWalkTime) * cameraSwayDistance;
            temPos.z = 0;

            temPos.Scale(mainCameraTransform.GetChild(0).localScale);
            mainCameraTransform.GetChild(0).localPosition = _playerDefaultHandPosition + temPos;
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

        var maxSpeed = maxMovementSpeed;
        if (_isOnGround && InputManager.Instance.InputSprint)
        {
            if (_characterEntity.GetStamina() < Time.deltaTime * 15)
            {
                _sprintPenalty = true;
            }

            if (_sprintPenalty)
            {
                if (_characterEntity.GetStamina() > 50)
                    _sprintPenalty = false;
            }
            else
            {
                _characterEntity.AddDeltaStamina(-Time.deltaTime * 15);
                maxSpeed *= sprintMultiplier;
                unitMovement *= sprintMultiplier;
            }
        }

        if (_isOnGround && InputManager.Instance.KeyDash && _characterEntity.GetStamina() >= 30)
        {
            _characterEntity.AddDeltaStamina(-30);
            _playerVelocity = _playerVelocity.normalized * dashMultiplier;
        }
        
        _walkAudio.isPlaying = _isOnGround && Mathf.Abs(unitMovement.magnitude) > float.Epsilon;
        
        var verticalVelocity = _playerVelocity.y; // backup
        _playerVelocity *= _isOnGround ? 0.9f : 0.999f; // friction

        // cap max speed
        _playerVelocity.y = 0;
        // if (_playerVelocity.magnitude > maxSpeed)
        //     _playerVelocity = _playerVelocity.normalized * maxSpeed;

        if (_playerVelocity.magnitude < maxSpeed)
            _playerVelocity += unitMovement * (movementSpeed * _movementControlRatio); // player control

        _playerVelocity.y = verticalVelocity; // restore

        /*
         *
         * Jump
         * 
         */
        if (_isOnGround && InputManager.Instance.InputJump)
        {
            _playerVelocity.y = Mathf.Sqrt(jumpHeight * gravity * 2);
            GetComponent<AudioSource>().PlayOneShot(jumpAudio);
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
        var ray = _mainCamera.ScreenPointToRay(_mainCamera.pixelRect.center);
        var size = Physics.RaycastNonAlloc(ray, hits);

        _playerInteractableRaycastResult = null;

        float minDistance = float.MaxValue;
        for (var i = 0; i < size; ++i)
        {
            if (hits[i].transform.gameObject.TryGetComponent(out PlayerInteractable hitResult))
            {
                var temDistance = (ray.origin - hits[i].point).magnitude;
                if (temDistance < minDistance)
                {
                    minDistance = temDistance;
                    _playerInteractableRaycastResultPosition = hits[i].point;
                    _playerInteractableRaycastResult = hitResult;
                }
            }
        }
    }


    void KeyInputHandler()
    {
        if (InputManager.Instance.KeyInteract)
        {
            if (_playerInteractableRaycastResult is not null)
            {
                _playerInteractableRaycastResult.OnInteract(gameObject, _playerInteractableRaycastResultPosition);
            }
        }
    }
}