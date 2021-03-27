using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 6f;
    public float groundDistance = 0.4f;
    public float vertVelocity;
    public float distance;
    public float JumpForce;

    public bool IsGrounded;
    public Transform RaycastPoint;
    private Rigidbody playerrb;

    void Start()
    {
        playerrb = gameObject.GetComponent<Rigidbody>();
    }
    void Update()
    {
        float dtime = Time.deltaTime;
        float movementX = Input.GetAxisRaw("Vertical");
        float movementZ = Input.GetAxisRaw("Horizontal");

        Vector3 movement = transform.right * movementX - transform.forward * movementZ;
        gameObject.transform.position += movement * (Speed * dtime);

        Vector3 originP = new Vector3(RaycastPoint.position.x, RaycastPoint.position.y, RaycastPoint.position.z);
        RaycastHit hit;
        Physics.Raycast(originP, Vector3.down, out hit);
        Debug.DrawRay(originP, Vector3.down);
        distance = hit.distance;

        if (hit.distance > groundDistance)
        {
            IsGrounded = false;
        }
        else
        {
            IsGrounded = true;
        }


        vertVelocity = playerrb.velocity.y;

        //velocity.y += Gravity * dtime;
        //gameObject.transform.position += velocity * dtime;
        

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            if (IsGrounded && vertVelocity == 0)
            {
                playerrb.AddForce(new Vector3(0,2f * JumpForce,0), ForceMode.Impulse);
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            Time.timeScale = 1;
        }
    }
}
