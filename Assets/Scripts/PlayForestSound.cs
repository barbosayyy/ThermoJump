using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayForestSound : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().instanceForestSound.start();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().instanceForestSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
