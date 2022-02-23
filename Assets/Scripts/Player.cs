using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.InputSystem;

//! Needs restructuring

public class Player : MonoBehaviour
{
    [Header("Movement")] public float playerSpeed;
    public float jumpForce;
    public GameObject gndCheck;
    public bool isGrounded;
    public bool isMoving = false;
    public bool canMove = true;
    public bool isPickingUpWeapon = false;
    public float jumpTimer = 3f;
    public bool canJump;
    public float groundDistance = 0.01f;
    public LayerMask sphereMask;

    public float sphereCastRadius = 0.25f;
    public float sphereCastDistance = 0.75f;

    public float hMovement;
    public float vMovement;
    public float mAxisX;
    public float mAxisY;
    public float maxVerticalSpeed = 10;
    public float maxHorizontalSpeed = 10;
    public float defaultSpeed = 40;
    public float airSpeed = 10;
    public float gravity = -1;
    private float accelSpeed;
    public GameObject rotationRight;
    public GameObject rotationLeft;
    public GameObject rotationNone;
    private Vector3 movement;
    private Vector3 x;
    private Vector3 z;

    [Header("Weapon Sway")]
    public float swayAmount = 0.02f;
    public float maxSwayAmount = 0.06f;
    public float smoothSwayAmount = 6f;
    public float swayRotationAmount = 4f;
    public float maxSwayRotationAmount = 5f;
    public float smoothSwayRotation = 12f;
    public bool rotationX = true;
    public bool rotationY = true;
    public bool rotationZ = true;
    public GameObject spawn1;
    public GameObject spawn2;
    
    private float _swayInputY;
    private float _swayInputX;
    private float _moveSwayX;
    private float _moveSwayY;

    [Header("FMOD")]

    public FMOD.Studio.EventInstance instance;
    public FMOD.Studio.EventInstance instanceForestSound;
    public FMOD.Studio.EventInstance instanceFallSounds;
    public float waitBetweenSteps;
    public bool hasMoved;
    public int currentSurface;

    [Header("Head Bobbing")]    

    private float _timer;

    private float _waveSlice;
    public Camera mainCam;
    private Vector3 _camTransform;
    public float bobbingSpeed;
    public float bobbingAmount;
    public float midpoint;

    //OTHER

    private InputMaster _inputMaster;

    private Vector3 _initialLauncherPos;
    private Quaternion _initialLauncherRotation;

    private GameObject _rocketLauncherPos;
    private Transform _rocketLauncherTransform;
    private Rigidbody _rigidBody;
    private RaycastHit _slopeHit;
    private float playerHeight = 1.9f;
    private Vector3 _slopeMoveDirection;
    private Vector3 _moveDirection;
    [SerializeField] private Transform orientation;
    private float _translateChange;
    private float _totalAxes;

    void Awake()
    {
        _inputMaster = new InputMaster();
        defaultSpeed = playerSpeed;
    }

    private void OnEnable()
    {
        _inputMaster.Enable();
    }

    private void OnDisable()
    {
        _inputMaster.Disable();
    }

    void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _rocketLauncherPos = GameObject.FindGameObjectWithTag("RocketLauncher");
        _rocketLauncherTransform = _rocketLauncherPos.transform;
        accelSpeed = playerSpeed;

        _initialLauncherRotation = _rocketLauncherTransform.localRotation;
        _initialLauncherPos = _rocketLauncherTransform.localPosition;

        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        hasMoved = false;

        instance = FMODUnity.RuntimeManager.CreateInstance("event:/FootSteps");
        instanceForestSound = FMODUnity.RuntimeManager.CreateInstance("event:/Nature");
        instanceFallSounds = FMODUnity.RuntimeManager.CreateInstance("event:/FallingSounds");

        instanceForestSound.start();
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, playerHeight / 2 + 1))
        {
            if (_slopeHit.normal != Vector3.up)
            {
                return true;
            }
        }
        return false;
    }
    
        
    void Update()
    {
        hMovement = _inputMaster.PlayerInput.Sideways.ReadValue<float>();
        vMovement = _inputMaster.PlayerInput.Forward.ReadValue<float>();
        mAxisX = Input.GetAxis("Mouse X");
        mAxisY = Input.GetAxis("Mouse Y");

        if (hMovement != 0 || vMovement != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        Transform from = _rocketLauncherTransform;
        Transform to;
        Transform original = rotationNone.transform;
        if (hMovement > 1)
        {
            to = rotationRight.transform;
            _rocketLauncherTransform.rotation = Quaternion.Lerp(from.rotation, to.rotation, Time.deltaTime * 10);
        }
        if (hMovement < -1)
        {
            to = rotationLeft.transform;
            _rocketLauncherTransform.rotation = Quaternion.Lerp(from.rotation, to.rotation, Time.deltaTime * 10);
        }
        if (hMovement == 0)
        {
            to = rotationNone.transform;
            _rocketLauncherTransform.rotation = Quaternion.Lerp(from.rotation, to.rotation, Time.deltaTime * 10);
        }

        LimitVerticalVelocity();
        LauncherSway();
        TeleportLocations();

        DetermineSurface();

        if (isMoving && isGrounded)
        {
            if (hasMoved == false && isPickingUpWeapon == false)
            {
                StartCoroutine(PlayFootSteps());
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //NEED TO FIX SOUNDS

        //if (Rb.velocity.y < -1 && isGrounded)
        //{
        //    instanceFallSounds.setParameterByName("Surfaces", currentSurface);
        //    instanceFallSounds.start();
        //}
    }

    void FixedUpdate()
    {   
        if (isPickingUpWeapon == false)
        {
            if (canMove == true)
            {
                Movement();
                DetectGround();
            }
        }
        
        if (isGrounded == true && canMove == true && isPickingUpWeapon == false)
        {
            HeadBob();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        //Gizmos.DrawWireCube(gameObject.transform.position + transform.forward * hit.distance, transform.localScale);
        //Gizmos.DrawCube(gndCheck.transform.position, new Vector3(0.4f, 0.4f, 0.4f));
    }

    public void Jump()
    {
        if (isGrounded == true && canJump == true)
        {
            _rigidBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            canJump = false;
            StartCoroutine(JumpTimer());
        }
    }

    IEnumerator JumpTimer()
    {
        yield return new WaitForSeconds(jumpTimer);
        yield return canJump = true;
    }

    void Movement()
    {
        x = transform.right * hMovement;
        z = transform.forward * vMovement;

        movement = (x + z).normalized * playerSpeed;

        _moveDirection = orientation.forward * vMovement + orientation.right * hMovement;
        
        _rigidBody.AddForce(movement);

        Vector3 velocity = _rigidBody.velocity;
        float vertVelocity = velocity.y;

        velocity.y = 0;

        velocity = Vector3.ClampMagnitude(velocity, maxHorizontalSpeed);

        velocity.y = vertVelocity;
        _rigidBody.velocity = velocity;

        if (isGrounded != true)
        {
            playerSpeed = airSpeed;
        }
        else
        {
            playerSpeed = defaultSpeed;
        }
        OnSlope();
        _slopeMoveDirection = Vector3.ProjectOnPlane(_moveDirection, _slopeHit.normal);
    }

    void LimitVerticalVelocity()
    {
        if (_rigidBody.velocity.y > maxVerticalSpeed)
        {
            _rigidBody.velocity = Vector3.ClampMagnitude(_rigidBody.velocity, maxVerticalSpeed);
        }
    }

    void DetectGround()
    {
        RaycastHit hit;

        if (Physics.SphereCast(gndCheck.transform.position, sphereCastRadius, Vector3.down, out hit, sphereCastDistance, sphereMask))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        if (canJump == false)
        {
            isGrounded = false;
        }
    }

    void LauncherSway()
    {
        _swayInputX = -mAxisX;
        _swayInputY = -mAxisY;

        _moveSwayX = Mathf.Clamp(_swayInputX * swayAmount, -maxSwayAmount, maxSwayAmount);
        _moveSwayY = Mathf.Clamp(_swayInputX * swayAmount, -maxSwayAmount, maxSwayAmount);

        Vector3 finalSwayPos = new Vector3(_moveSwayX, _moveSwayY, 0);

        _rocketLauncherTransform.localPosition = Vector3.Lerp(_rocketLauncherTransform.localPosition, finalSwayPos + _initialLauncherPos, Time.deltaTime * smoothSwayAmount);

        float swayTiltY = Mathf.Clamp(_swayInputX * swayRotationAmount, -maxSwayRotationAmount, maxSwayRotationAmount);
        float swayTiltX = Mathf.Clamp(_swayInputY * swayRotationAmount, -maxSwayRotationAmount, maxSwayRotationAmount);

        Quaternion finalSwayRotation = Quaternion.Euler(new Vector3(rotationX ? -swayTiltX : 0f, rotationY ? swayTiltY : 0f, rotationZ ? swayTiltY : 0));

        _rocketLauncherTransform.localRotation = Quaternion.Slerp(_rocketLauncherTransform.localRotation, finalSwayRotation * _initialLauncherRotation, Time.deltaTime * smoothSwayRotation);
    }

    void TeleportLocations()
    {
        if (Input.GetKeyDown(KeyCode.L) && Application.isEditor)
        {
            gameObject.transform.position = spawn1.transform.position;
        }
        if (Input.GetKeyDown(KeyCode.K) && Application.isEditor)
        {
            gameObject.transform.position = spawn2.transform.position;
        }
    }

    void HeadBob()
    {
        _waveSlice = 0f;
        

        if(Mathf.Abs(hMovement) == 0 && Mathf.Abs(vMovement) == 0)
        {
            _timer = 0f;
        }
        else
        {
            _waveSlice = Mathf.Sin(_timer);
            _timer = _timer + bobbingSpeed;
            if (_timer > Mathf.PI * 2)
            {
                _timer = _timer - (Mathf.PI * 2);
            }
        }
        if (_waveSlice != 0)
        {
            _translateChange = _waveSlice * bobbingAmount;
            _totalAxes = Mathf.Abs(hMovement) + Mathf.Abs(vMovement);
            _totalAxes = Mathf.Clamp(_totalAxes, 0f, 1f);
            _translateChange = _totalAxes * _translateChange;
            _camTransform.y = midpoint + _translateChange;
            mainCam.transform.localPosition = _camTransform;
        }
        else
        {
            _camTransform.y = midpoint;
            mainCam.transform.localPosition = _camTransform;
        }
    }

    void OnDrawGizmos()
    {
        Vector3 gndCheckVec = gndCheck.transform.position;
        Gizmos.DrawSphere(gndCheckVec, sphereCastRadius);
        Gizmos.DrawRay(gndCheckVec, new Vector3(0,-0.75f,0));
    }

    void DetermineSurface()
    {
        RaycastHit[] hit;

        hit = Physics.RaycastAll(gndCheck.transform.position, Vector3.down, 0.75f);

        foreach (RaycastHit rayhit in hit)
        {
            if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Grass"))
            {
                currentSurface = 2;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Stone"))
            {
                currentSurface = 0;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Wood"))
            {
                currentSurface = 1;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Brick"))
            {
                currentSurface = 3;
            }
        }
    }

    IEnumerator PlayFootSteps()
    {
        hasMoved = true;
        instance.setParameterByName("Surfaces", currentSurface);
        instance.start();
        yield return new WaitForSeconds(waitBetweenSteps);
        yield return hasMoved = false;
    }


}
