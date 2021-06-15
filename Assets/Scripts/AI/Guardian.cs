using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class Guardian : MonoBehaviour
{
    public GameObject player;
    GameObject startPosObj;
    public GameObject projectile;
    public GameObject pivot;
    public bool canSee;
    public bool canShoot;
    public float speed;
    public float rCmaxDist;
    public float maxDistance;
    public float distanceFromStart;
    public float rotationDamping;
    public float shootTimer;
    private RaycastHit hit;

    private void Start()
    {
        canShoot = true;
        startPosObj = new GameObject();
        startPosObj.transform.position = gameObject.transform.position;
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, rCmaxDist) && hit.collider.CompareTag("Player"))
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

        Vector3 direction = player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
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
        {
            Quaternion rotation = Quaternion.LookRotation(player.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);
        }
        if (distanceFromStart > maxDistance)
        {
            Task.current.Succeed();
        }
    }

    [Task]
    void Shoot()
    {
        lookAtPlayer();
        if (canShoot)
        {
            StartCoroutine(ShootProjectile());
        }
        Task.current.Succeed();
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

    void lookAtPlayer()
    {
        
    }

    IEnumerator ShootProjectile()
    {
        canShoot = false;
        GameObject projectileTemp;
        projectileTemp = GameObject.Instantiate(projectile, gameObject.transform.localPosition, Quaternion.identity);
        projectileTemp.GetComponent<EnemyProjectile>().pivot = pivot;
        projectileTemp.GetComponent<EnemyProjectile>().enemy = gameObject;
        yield return new WaitForSeconds(shootTimer);
        yield return canShoot = true;
    }
}
