using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : MonoBehaviour
{
    public Transform startFrame;
    public Transform startingPoint;
    public Transform endPoint;
    public float waitTime;
    public bool moveUp;
    public bool canMove = true;
    public float moveSpeed = 1;

    public FMOD.Studio.EventInstance instanceCollumn;


    private void Start()
    {
        transform.position = startFrame.position;

        instanceCollumn = FMODUnity.RuntimeManager.CreateInstance("event:/StoneCollumn");
        instanceCollumn.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(endPoint.transform.position));
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
        instanceCollumn.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instanceCollumn.start();
        canMove = true;
        moveUp = !moveUp;
    }
}