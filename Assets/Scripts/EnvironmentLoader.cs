using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class EnvironmentLoader : MonoBehaviour
{
    private Light sun;
    private Volume skyVol;
    private Fog fog;
    private GameObject environmentCastle;
    private GameObject environmentForest;
    private float currentValue;
    [SerializeField]
    float sunEnterValue;
    [SerializeField]
    float fogEnterValue;
    [SerializeField]
    float sunExitValue;
    [SerializeField]
    float fogExitValue;
    void Start()
    {
        sun = GameObject.FindGameObjectWithTag("Sun").GetComponent<Light>();
        skyVol = GameObject.FindGameObjectWithTag("SfVolume").GetComponent<Volume>();
        StartCoroutine (LoadEnvironment());
    }

    void Update()
    {
        //currentValue = sun.intensity;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            sun.intensity = sunEnterValue;
            skyVol.profile.TryGet<Fog>(out fog);
            fog.meanFreePath.value = fogEnterValue;
            environmentForest.SetActive(false);
            environmentCastle.SetActive(true);
        }

    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            sun.intensity = sunExitValue;
            skyVol.profile.TryGet<Fog>(out fog);
            fog.meanFreePath.value = fogExitValue;
            environmentForest.SetActive(true);
            environmentCastle.SetActive(false);
        }
    }

    IEnumerator LoadEnvironment()
    {
        environmentCastle = GameObject.FindGameObjectWithTag("EnvironmentCastle");
        environmentForest = GameObject.FindGameObjectWithTag("EnvironmentForest");
        yield return new WaitForSeconds(1f);
        environmentCastle.SetActive(false);
        yield break;
        
    }
}
