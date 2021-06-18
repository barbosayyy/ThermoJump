using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayWaterSound : MonoBehaviour
{
    public FMOD.Studio.EventInstance instanceWater;

    private void Start()
    {
        instanceWater = FMODUnity.RuntimeManager.CreateInstance("event:/Water");

        instanceWater.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform.position));

        instanceWater.start();
    }
}
