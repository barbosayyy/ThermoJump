using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class ShootRocket : MonoBehaviour
{
    public GameObject pivot;
    public bool hasShot = false;
    public float waitTime = 3f;
    private float shootingCooldown;
    public bool canShoot;
    public bool hasWeapon;
    public GameObject rocketLauncher;
    public Volume m_Volume;
    private VolumeProfile profile;

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
        profile = m_Volume.sharedProfile;
        
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

    public void PickUpFunc()
    {
        StartCoroutine(PickUpEvent());
    }

    IEnumerator PickUpEvent()
    {
        DepthOfField dph;
        if (profile.TryGet<DepthOfField>(out dph))
        {
            dph.active = true;
        }
        rocketLauncher.SetActive(true);
        Player player = gameObject.GetComponent<Player>();
        player.canMove = false;
        player.hasMoved = true;
        yield return new WaitForSecondsRealtime(4f);
        player.hasMoved = false;
        player.canMove = true;
        if (profile.TryGet<DepthOfField>(out dph))
        {
            dph.active = false;
        }
        yield return canShoot = true;
    }
}
