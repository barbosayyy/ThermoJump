using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootRocket : MonoBehaviour
{
    public GameObject pivot;
    public bool hasShot = false;
    public float waitTime = 3f;
    private float shootingCooldown;

    private GameObject rocketPrefab;
    private Vector3 pivotpos;



    void Start()
    {
        pivotpos = pivot.transform.position;
        rocketPrefab = Resources.Load<GameObject>("rocket");
    }

    void Update()
    {
        if (hasShot == true)
        {
            shootingCooldown -= Time.deltaTime;
        }
        
        if (shootingCooldown <= 0)
        {
            hasShot = false;
        }
    }

    public void Shoot()
    {
        if(hasShot == false)
        {
            GameObject.Instantiate(rocketPrefab, pivotpos, Quaternion.identity);
            Debug.Log("shot rocket");
            shootingCooldown = waitTime;
            hasShot = true;
        }
    }
}
