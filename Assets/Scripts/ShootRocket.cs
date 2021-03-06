using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class ShootRocket : MonoBehaviour
{
    public GameObject pivot;
    public bool hasShot = false;
    private float cooldownValue;

    public bool canShoot;
    public bool hasWeapon;
    public bool isPaused;
    private bool hasPlayed;
    public GameObject rocketLauncher;
    public Volume m_Volume;
    private VolumeProfile profile;

    public FMOD.Studio.EventInstance instanceMusic;
    public FMOD.Studio.EventInstance instanceShoot;

    public PlayerStats playerStats;
    public Animator playerAnimator;
    private GameObject rocketPrefab;
    private Vector3 pivotpos;

    void Awake()
    {
        playerAnimator = gameObject.GetComponentInChildren<Animator>();
    }

    void Start()
    {
        playerStats = gameObject.GetComponent<PlayerStats>();
        pivotpos = pivot.transform.position;
        cooldownValue = playerStats.weaponCooldown;
        rocketPrefab = Resources.Load<GameObject>("rocket");
        rocketLauncher.SetActive(false);
        hasWeapon = false;
        hasPlayed = false;
        canShoot = false;
        profile = m_Volume.sharedProfile;
        instanceMusic = FMODUnity.RuntimeManager.CreateInstance("event:/RocketLauncherFound");
        instanceShoot = FMODUnity.RuntimeManager.CreateInstance("event:/Shoot");
    }

    void Update()
    {
        if (hasShot == true)
        {
            playerStats.weaponCooldown += Time.deltaTime;
        }
        
        if (playerStats.weaponCooldown >= cooldownValue)
        {
            hasShot = false;
        }

        if (hasWeapon && hasPlayed == false)
        {
            StartCoroutine(PlayMusic());
        }
    }

    public void Shoot()
    {
        if (!isPaused)
        {
            if (hasWeapon)
            {
                if (canShoot)
                {
                    if (hasShot == false)
                    {
                        instanceShoot.start();
                        playerAnimator.SetTrigger("ShootRocket");
                        GameObject.Instantiate(rocketPrefab, pivotpos, Quaternion.identity);
                        Debug.Log("shot rocket");
                        playerStats.weaponCooldown = 0;
                        hasShot = true;
                        //PLAY FMOD CLIP
                    }
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
        player.isPickingUpWeapon = true;
        yield return new WaitForSecondsRealtime(4f);
        if (profile.TryGet<DepthOfField>(out dph))
        {
            dph.active = false;
        }
        player.isPickingUpWeapon = false;
        yield return canShoot = true;
    }

    IEnumerator PlayMusic()
    {
        hasPlayed = true;
        yield return new WaitForSeconds(5);
        instanceMusic.start();
        yield break;
    }

}
