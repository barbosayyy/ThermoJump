using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : MonoBehaviour
{
    public Transform startingPoint;
    public Transform endPoint;
    public float waitTime;
    public bool moveUp;
    public bool canMove = true;
    public float moveSpeed = 1;

    private void Start()
    {
        transform.position = startingPoint.position;
    }

    private void Update()
    {
        if (!canMove)
        {
            return;
        }

        Vector3 moveDirection = Vector3.up;

        if (!moveUp)
        {
            moveDirection = -moveDirection;
        }

        gameObject.transform.Translate(moveDirection * Time.deltaTime * moveSpeed, Space.Self);
        
        if ((!moveUp && transform.position.y <= endPoint.position.y) || (moveUp && transform.position.y >= startingPoint.position.y))
        {
            StartCoroutine(Switch());
        }
    }

    IEnumerator Switch()
    {
        canMove = false;
        yield return new WaitForSeconds(waitTime);
        canMove = true;
        moveUp = !moveUp;
    }
}