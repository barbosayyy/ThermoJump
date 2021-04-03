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
    public float jumpTimer = 3f;
    public  bool canJump;
    public float groundDistance = 0.01f;
    public float hMovement;
    public float vMovement;
    public float mAxisX;
    public float mAxisY;

    private Rigidbody Rb;
    void Start()
    {
        Rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        hMovement = Input.GetAxisRaw("Horizontal") * playerSpeed;
        vMovement = Input.GetAxisRaw("Vertical") * playerSpeed;
        mAxisX = Input.GetAxis("Mouse X");
        mAxisY = Input.GetAxis("Mouse Y");
    }

    void FixedUpdate()
    {

        Vector3 movePos = transform.right * hMovement + transform.forward * vMovement;
        Vector3 newMovePos = new Vector3(movePos.x, Rb.velocity.y, movePos.z);

        Rb.velocity = newMovePos;

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
