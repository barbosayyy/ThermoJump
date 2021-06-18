using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyExplosion : MonoBehaviour
{
    public float explosionTime;
    void Start()
    {
        StartCoroutine(WaitExplosionTime());
    }

    IEnumerator WaitExplosionTime()
    {
        yield return new WaitForSeconds(explosionTime);
        Destroy(gameObject);
        yield break;
    }
}
