using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public ShootRocket sRPlayer;
    private FMOD.Studio.EventInstance _instance;
    
    private void Awake()
    {
        _instance = FMODUnity.RuntimeManager.CreateInstance("event:/GunMovement");
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            PickUp(col);
        }
    }

    void PickUp(Collider col)
    {
            sRPlayer = col.GetComponent<ShootRocket>();
            sRPlayer.hasWeapon = true;
            sRPlayer.PickUpFunc();
            gameObject.SetActive(false);
            _instance.start();
    }
}
