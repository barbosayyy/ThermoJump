using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class EnvironmentLoader : MonoBehaviour
{
    private Light _sun;
    private Volume _skyVol;
    private Fog _fog;
    [SerializeField]
    private GameObject environmentCastle;
    [SerializeField]
    private GameObject environmentForest;
    private float _currentValue;
    [SerializeField]
    private float sunEnterValue;
    [SerializeField]
    private float fogEnterValue;
    [SerializeField]
    private float sunExitValue;
    [SerializeField]
    private float fogExitValue;
    void Start()
    {
        _sun = GameObject.FindGameObjectWithTag("Sun").GetComponent<Light>();
        _skyVol = GameObject.FindGameObjectWithTag("SfVolume").GetComponent<Volume>();
        StartCoroutine (LoadEnvironment());
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            EnterCastleLevel();
        }

    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            ExitCastleLevel();
        }
    }

    void EnterCastleLevel()
    {
        _sun.intensity = sunEnterValue;
        _skyVol.profile.TryGet<Fog>(out _fog);
        _fog.meanFreePath.value = fogEnterValue;
        environmentForest.SetActive(false);
        environmentCastle.SetActive(true);
    }

    void ExitCastleLevel()
    {
        _sun.intensity = sunExitValue;
        _skyVol.profile.TryGet<Fog>(out _fog);
        _fog.meanFreePath.value = fogExitValue;
        environmentForest.SetActive(true);
        environmentCastle.SetActive(false);
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
