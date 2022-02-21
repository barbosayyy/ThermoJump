using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class ShootRocket : MonoBehaviour
{
    public GameObject pivot;
    public bool hasShot = false;
    private float _cooldownValue;

    public bool canShoot;
    public bool hasWeapon;
    public bool isPaused;
    private bool _hasPlayed;
    public GameObject rocketLauncher;
    public Volume m_Volume;
    private VolumeProfile _profile;

    public FMOD.Studio.EventInstance instanceMusic;
    public FMOD.Studio.EventInstance instanceShoot;

    public PlayerStats playerStats;
    public Animator playerAnimator;
    private GameObject _rocketPrefab;
    private Vector3 _pivotPosition;

    void Awake()
    {
        playerAnimator = gameObject.GetComponentInChildren<Animator>();
    }

    void Start()
    {
        playerStats = gameObject.GetComponent<PlayerStats>();
        _pivotPosition = pivot.transform.position;
        _cooldownValue = playerStats.weaponCooldown;
        _rocketPrefab = Resources.Load<GameObject>("rocket");
        rocketLauncher.SetActive(false);
        hasWeapon = false;
        _hasPlayed = false;
        canShoot = false;
        _profile = m_Volume.sharedProfile;
        instanceMusic = FMODUnity.RuntimeManager.CreateInstance("event:/RocketLauncherFound");
        instanceShoot = FMODUnity.RuntimeManager.CreateInstance("event:/Shoot");
    }

    void Update()
    {
        if (hasShot == true)
        {
            playerStats.weaponCooldown += Time.deltaTime;
        }
        
        if (playerStats.weaponCooldown >= _cooldownValue)
        {
            hasShot = false;
        }

        if (hasWeapon && _hasPlayed == false)
        {
            StartCoroutine(PlayMusic());
        }
    }

    public void Shoot() //Accessed by Input System
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
                        GameObject.Instantiate(_rocketPrefab, _pivotPosition, Quaternion.identity);
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
        if (_profile.TryGet<DepthOfField>(out dph))
        {
            dph.active = true;
        }
        rocketLauncher.SetActive(true);
        Player player = gameObject.GetComponent<Player>();
        player.isPickingUpWeapon = true;
        yield return new WaitForSecondsRealtime(4f);
        if (_profile.TryGet<DepthOfField>(out dph))
        {
            dph.active = false;
        }
        player.isPickingUpWeapon = false;
        yield return canShoot = true;
    }

    IEnumerator PlayMusic()
    {
        _hasPlayed = true;
        yield return new WaitForSeconds(5);
        instanceMusic.start();
        yield break;
    }

}
