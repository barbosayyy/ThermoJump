using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyExplosion : MonoBehaviour
{
    private float vfxDuration;
    void Start()
    {
        vfxDuration = gameObject.GetComponent<ParticleSystem>().main.duration;
        Destroy(gameObject, vfxDuration);
    }

    void Update()
    {
        
    }
}
