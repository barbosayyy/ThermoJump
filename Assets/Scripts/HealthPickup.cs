using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float hpAdd;
    public float rotationsPerMinute;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>().health += hpAdd;
            if (other.GetComponent<PlayerStats>().health > 100)
            {
                other.GetComponent<PlayerStats>().health = 100;
            }
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
             transform.Rotate(0, 0, 6.0f * rotationsPerMinute * Time.deltaTime);
    }
}
