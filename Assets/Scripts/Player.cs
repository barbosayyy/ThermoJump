using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float playerSpeed;
    public float jumpForce;
    public GameObject gndCheck;
    public bool isGrounded;
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
    public float accelSpeed = 40;
    public float airSpeed = 10;
    public float gravity = -1;
    private float initialSpeed;
    public GameObject rotationRight;
    public GameObject rotationLeft;
    public GameObject rotationNone;

    private GameObject rocketLauncherPos;
    private Rigidbody Rb;
    void Start()
    {
        Rb = gameObject.GetComponent<Rigidbody>();
        rocketLauncherPos = GameObject.FindGameObjectWithTag("RocketLauncher");
        initialSpeed = playerSpeed;
    }
        
    void Update()
    {
        hMovement = Input.GetAxisRaw("Horizontal");
        vMovement = Input.GetAxisRaw("Vertical");
        mAxisX = Input.GetAxis("Mouse X");
        mAxisY = Input.GetAxis("Mouse Y");

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
    }

    void FixedUpdate()
    {
        MovementAF();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(gndCheck.transform.position, new Vector3(0.4f, 0.4f, 0.4f));
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

    void MovementTransform()
    {
        Vector3 movePos = transform.right * hMovement + transform.forward * vMovement;
        Vector3 newMovePos = new Vector3(movePos.x, Rb.velocity.y, movePos.z);

        if (canMove)
        {
            Rb.velocity = newMovePos;
        }

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

    void MovementAF()
    {
        Vector3 x = transform.right * hMovement;
        Vector3 z = transform.forward * vMovement;

        Vector3 movement = (x + z).normalized * playerSpeed;

        Rb.AddForce(movement);

        if (Rb.velocity.x < 1 && Rb.velocity.x > -1 || Rb.velocity.z < 1 && Rb.velocity.z > -1)
        {
            playerSpeed = accelSpeed;
        }
        else
        {
            playerSpeed = initialSpeed;
        }

        if (isGrounded != true)
        {
            playerSpeed = airSpeed;
        }

        if (Rb.velocity.x > maxHorizontalSpeed)
        {
            Vector3 vel = Rb.velocity.normalized * maxHorizontalSpeed;
            vel.y = Rb.velocity.y;
            Rb.velocity = vel;
            //Rb.velocity = Vector3.ClampMagnitude(Rb.velocity, maxHorizontalSpeed);
        }
        else if (Rb.velocity.x < -maxHorizontalSpeed)
        {
            Vector3 vel = Rb.velocity.normalized * maxHorizontalSpeed;
            vel.y = Rb.velocity.y;
            Rb.velocity = vel;
            //Rb.velocity = Vector3.ClampMagnitude(Rb.velocity, maxHorizontalSpeed);
        }

        if (Rb.velocity.z > maxHorizontalSpeed)
        {
            Vector3 vel = Rb.velocity.normalized * maxHorizontalSpeed;
            vel.y = Rb.velocity.y;
            Rb.velocity = vel;
            //Rb.velocity = Vector3.ClampMagnitude(Rb.velocity, maxHorizontalSpeed);
        }
        else if (Rb.velocity.z < -maxHorizontalSpeed)
        {
            Vector3 vel = Rb.velocity.normalized * maxHorizontalSpeed;
            vel.y = Rb.velocity.y;
            Rb.velocity = vel;
            //Rb.velocity = Vector3.ClampMagnitude(Rb.velocity, maxHorizontalSpeed);
        }

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

    void LimitVerticalVelocity()
    {
        if (Rb.velocity.y > maxVerticalSpeed)
        {
            Rb.velocity = Vector3.ClampMagnitude(Rb.velocity, maxVerticalSpeed);
        }
    }
}
