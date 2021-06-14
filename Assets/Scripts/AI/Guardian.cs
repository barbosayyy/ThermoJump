using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class Guardian : MonoBehaviour
{
    public GameObject player;
    GameObject startPosObj;
    public Transform shootPoint;
    public bool canSee;
    public float speed;
    public float rCmaxDist;
    public float maxDistance;
    public float distanceFromStart;

    private void Start()
    {
        startPosObj = new GameObject();
        startPosObj.transform.position = gameObject.transform.position;
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out RaycastHit hit, rCmaxDist) && hit.collider.CompareTag("Player"))
        {
            canSee = true;
        }
        else
        {
            canSee = false;
        }
    }

    private void Update()
    {
        distanceFromStart = Vector3.Distance(gameObject.transform.position, startPosObj.transform.position);
    }

    [Task]
    bool CanSeePlayer()
    {
        if (canSee == true)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    [Task]
    void ApproachPlayer()
    {
        transform.position = Vector3.MoveTowards(gameObject.transform.position, player.transform.position, speed);
        if (distanceFromStart > maxDistance)
        {
            Task.current.Succeed();
        }
    }

    [Task]
    void Shoot()
    {

    }

    [Task]
    void GoToStartPosition()
    {
        transform.position = Vector3.MoveTowards(gameObject.transform.position, startPosObj.transform.position, speed);
        if (gameObject.transform.position == startPosObj.transform.position)
        {
            Task.current.Succeed();
        }
    }
}
