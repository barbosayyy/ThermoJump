using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySplashSound : MonoBehaviour
{
    public FMOD.Studio.EventInstance instanceSplash;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            instanceSplash = FMODUnity.RuntimeManager.CreateInstance("event:/PlayerInWater");

            instanceSplash.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform.position));

            instanceSplash.start();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            instanceSplash = FMODUnity.RuntimeManager.CreateInstance("event:/PlayerInWater");

            instanceSplash.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform.position));

            instanceSplash.start();
        }
    }
}
