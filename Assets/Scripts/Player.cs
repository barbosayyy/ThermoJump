﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float playerSpeed;
    public float jumpForce;
    public GameObject gndCheck;
    public bool isGrounded;
    public bool isMoving = false;
    public bool canMove = true;
    public float jumpTimer = 3f;
    public  bool canJump;
    public float groundDistance = 0.01f;
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
    //Launcher Sway
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

    [Header("Head Bobbing")]    

    private float timer;
    public Camera mainCam;
    private Vector3 camTransform;
    public float bobbingSpeed;
    public float bobbingAmount;
    public float midpoint;

    //Other privates

    private InputMaster inputMaster;

    private Vector3 initialLauncherPos;
    private Quaternion initialLauncherRotation;

    private GameObject rocketLauncherPos;
    private Rigidbody Rb;

    void Awake()
    {
        inputMaster = new InputMaster();
        defaultSpeed = playerSpeed;
    }

    private void OnEnable()
    {
        inputMaster.Enable();
    }

    private void OnDisable()
    {
        inputMaster.Disable();
    }

    void Start()
    {
        Rb = gameObject.GetComponent<Rigidbody>();
        rocketLauncherPos = GameObject.FindGameObjectWithTag("RocketLauncher");
        accelSpeed = playerSpeed;

        initialLauncherRotation = rocketLauncherPos.transform.localRotation;
        initialLauncherPos = rocketLauncherPos.transform.localPosition;

        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
        
    void Update()
    {
        // Read Input 
        hMovement = inputMaster.PlayerInput.Sideways.ReadValue<float>();
        vMovement = inputMaster.PlayerInput.Forward.ReadValue<float>();
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

        Transform from = rocketLauncherPos.transform;
        Transform to;
        Transform original = rotationNone.transform;
        if (hMovement > 1)
        {
            to = rotationRight.transform;
            rocketLauncherPos.transform.rotation = Quaternion.Lerp(from.rotation, to.rotation, Time.deltaTime * 10);
        }
        if (hMovement < -1)
        {
            to = rotationLeft.transform;
            rocketLauncherPos.transform.rotation = Quaternion.Lerp(from.rotation, to.rotation, Time.deltaTime * 10);
        }
        if (hMovement == 0)
        {
            to = rotationNone.transform;
            rocketLauncherPos.transform.rotation = Quaternion.Lerp(from.rotation, to.rotation, Time.deltaTime * 10);
        }

        LimitVerticalVelocity();
        LauncherSway();
        TeleportLocations();
        if (isGrounded == true)
        {
            HeadBob();
        }
    }

    void FixedUpdate()
    {   
        MovementAF();
        DetectGround();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        //Gizmos.DrawWireCube(gameObject.transform.position + transform.forward * hit.distance, transform.localScale);
        //Gizmos.DrawCube(gndCheck.transform.position, new Vector3(0.4f, 0.4f, 0.4f));
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.collider != null && isGrounded == false)
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
        if (isGrounded == true)
        {
            canMove = true;
        }
    }

    public void Jump()
    {
        if (isGrounded == true && canJump == true)
        {
            Rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
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

    void MovementAF()
    {
        Vector3 x = transform.right * hMovement;
        Vector3 z = transform.forward * vMovement;

        Vector3 movement = (x + z).normalized * playerSpeed;

        Rb.AddForce(movement);

        Vector3 velocity = Rb.velocity;
        float vertVelocity = velocity.y;

        velocity.y = 0;

        velocity = Vector3.ClampMagnitude(velocity, maxHorizontalSpeed);

        velocity.y = vertVelocity;
        Rb.velocity = velocity;

        if (isGrounded != true)
        {
            playerSpeed = airSpeed;
        }
        else
        {
            playerSpeed = defaultSpeed;
        }
    }

    // Velocity Limiter

    void LimitVerticalVelocity()
    {
        if (Rb.velocity.y > maxVerticalSpeed)
        {
            Rb.velocity = Vector3.ClampMagnitude(Rb.velocity, maxVerticalSpeed);
        }
    }

    void DetectGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(gndCheck.transform.position, -gndCheck.transform.up, out hit, Mathf.Infinity))
        {
            if (hit.distance < groundDistance)
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }

        if (canJump == false)
        {
            isGrounded = false;
        }
    }

    void LauncherSway()
    {
        float swayInputX = -mAxisX;
        float swayInputY = -mAxisY;

        float moveSwayX = Mathf.Clamp(swayInputX * swayAmount, -maxSwayAmount, maxSwayAmount);
        float moveSwayY = Mathf.Clamp(swayInputX * swayAmount, -maxSwayAmount, maxSwayAmount);

        Vector3 finalSwayPos = new Vector3(moveSwayX, moveSwayY, 0);

        rocketLauncherPos.transform.localPosition = Vector3.Lerp(rocketLauncherPos.transform.localPosition, finalSwayPos + initialLauncherPos, Time.deltaTime * smoothSwayAmount);

        float swayTiltY = Mathf.Clamp(swayInputX * swayRotationAmount, -maxSwayRotationAmount, maxSwayRotationAmount);
        float swayTiltX = Mathf.Clamp(swayInputY * swayRotationAmount, -maxSwayRotationAmount, maxSwayRotationAmount);

        Quaternion finalSwayRotation = Quaternion.Euler(new Vector3(rotationX ? -swayTiltX : 0f, rotationY ? swayTiltY : 0f, rotationZ ? swayTiltY : 0));

        rocketLauncherPos.transform.localRotation = Quaternion.Slerp(rocketLauncherPos.transform.localRotation, finalSwayRotation * initialLauncherRotation, Time.deltaTime * smoothSwayRotation);
    }

    void TeleportLocations()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            gameObject.transform.position = spawn1.transform.position;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            gameObject.transform.position = spawn2.transform.position;
        }

    }

    void HeadBob()
    {
        float waveslice = 0f;
        

        if(Mathf.Abs(hMovement) == 0 && Mathf.Abs(vMovement) == 0)
        {
            timer = 0f;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            timer = timer + bobbingSpeed;
            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }
        }
        if (waveslice != 0)
        {
            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Abs(hMovement) + Mathf.Abs(vMovement);
            totalAxes = Mathf.Clamp(totalAxes, 0f, 1f);
            translateChange = totalAxes * translateChange;
            camTransform.y = midpoint + translateChange;
            mainCam.transform.localPosition = camTransform;
        }
        else
        {
            camTransform.y = midpoint;
            mainCam.transform.localPosition = camTransform;
        }
    }
}
