using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class Guardian : MonoBehaviour
{
    public GameObject player;
    public Transform startPosition;
    public Transform shootPoint;

    [Task]
    bool CanSeePlayer()
    {
        if (player.active == true)
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

    }

    [Task]
    void Shoot()
    {

    }

    [Task]
    void GoToStartPosition()
    {

    }
}
