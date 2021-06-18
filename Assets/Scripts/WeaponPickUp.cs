using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public ShootRocket sRPlayer;
    public FMOD.Studio.EventInstance instance;
    

    private void Awake()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance("event:/GunMovement");
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            sRPlayer = col.GetComponent<ShootRocket>();
            sRPlayer.hasWeapon = true;
            sRPlayer.PickUpFunc();
            gameObject.SetActive(false);
            instance.start();
        }
    }
}
