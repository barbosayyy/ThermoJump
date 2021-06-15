using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public ShootRocket sRPlayer;

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            sRPlayer = col.GetComponent<ShootRocket>();

            sRPlayer.hasWeapon = true;
            sRPlayer.PickUpFunc();
            gameObject.SetActive(false);
        }
    }
}
