using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootRocket : MonoBehaviour
{
    public GameObject pivot;
    public bool hasShot = false;
    public float waitTime = 3f;
    private float shootingCooldown;
    public bool canShoot;
    public bool hasWeapon;
    public GameObject rocketLauncher;

    public Animator playerAnimator;
    private GameObject rocketPrefab;
    private Vector3 pivotpos;

    void Awake()
    {
        playerAnimator = gameObject.GetComponentInChildren<Animator>();
    }

    void Start()
    {
        pivotpos = pivot.transform.position;
        rocketPrefab = Resources.Load<GameObject>("rocket");
        rocketLauncher.SetActive(false);
        hasWeapon = false;
        canShoot = false;
        
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

        if (hasWeapon)
        {
            rocketLauncher.SetActive(true);
            canShoot = true;
        }
    }

    public void Shoot()
    {
        if (hasWeapon)
        {
            if (canShoot)
            {
                if (hasShot == false)
                {
                    playerAnimator.SetTrigger("ShootRocket");
                    GameObject.Instantiate(rocketPrefab, pivotpos, Quaternion.identity);
                    Debug.Log("shot rocket");
                    shootingCooldown = waitTime;
                    hasShot = true;
                    //PLAY FMOD CLIP
                }
            }
        }
    }
}
