using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float playerSpeed;
    public float jumpForce;
    public float strafeVelocity;
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
    public GameObject rotationRight;
    public GameObject rotationLeft;
    public GameObject rotationNone;

    private GameObject rocketLauncherPos;
    private Rigidbody Rb;
    void Start()
    {
        Rb = gameObject.GetComponent<Rigidbody>();
        rocketLauncherPos = GameObject.FindGameObjectWithTag("RocketLauncher");
    }

    void Update()
    {
        hMovement = Input.GetAxisRaw("Horizontal") * playerSpeed;
        vMovement = Input.GetAxisRaw("Vertical") * playerSpeed;
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
    }

    void FixedUpdate()
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
}
