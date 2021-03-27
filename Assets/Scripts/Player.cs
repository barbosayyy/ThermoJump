using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float playerSpeed;
    public float jumpForce;
    public GameObject gndCheck;
    public bool isGrounded;
    public float jumpTimer = 3f;
    public  bool canJump;
    public float groundDistance = 0.01f;

    private Rigidbody Rb;
    void Start()
    {
        Rb = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float hMovement = Input.GetAxisRaw("Horizontal") * playerSpeed;
        float vMovement = Input.GetAxisRaw("Vertical") * playerSpeed;

        Vector3 movePos = transform.right * hMovement + transform.forward * vMovement;
        Vector3 newMovePos = new Vector3(movePos.x, Rb.velocity.y, movePos.z);

        Rb.velocity = newMovePos;

        RaycastHit hit;

        if (Physics.Raycast(gndCheck.transform.position, -gndCheck.transform.up, out hit, Mathf.Infinity))
        {
            if (isGrounded == false && hit.distance < groundDistance)
            {
                isGrounded = true;
            }
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
