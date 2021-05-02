using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentLoader : MonoBehaviour
{
    private Light sun;
    private GameObject environmentCastle;
    private float currentValue;
    [SerializeField]
    float enterValue;
    [SerializeField]
    float exitValue;
    void Start()
    {
        sun = GameObject.FindGameObjectWithTag("Sun").GetComponent<Light>();
        environmentCastle = GameObject.FindGameObjectWithTag("EnvironmentCastle");
    }

    void Update()
    {
        //currentValue = sun.intensity;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            sun.intensity = enterValue;
            environmentCastle.SetActive(true);
        }

    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            sun.intensity = exitValue;
            environmentCastle.SetActive(false);
        }
    }
}
