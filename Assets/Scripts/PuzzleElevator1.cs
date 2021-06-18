using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleElevator1 : MonoBehaviour
{
    public GameObject pressurePlateObj;
    public PressurePlate pressurePlate;
    public bool isDefault;
    public GameObject start;
    public GameObject end;
    public float speed;

    public FMOD.Studio.EventInstance instanceElevator;

    private void Start()
    {
        isDefault = true;
        pressurePlate = pressurePlateObj.GetComponent<PressurePlate>();
        pressurePlate.OnActivated.AddListener(gameObject.GetComponent<PuzzleElevator1>().Activate);
        pressurePlate.OnDeactivated.AddListener(gameObject.GetComponent<PuzzleElevator1>().Deactivate);

        instanceElevator = FMODUnity.RuntimeManager.CreateInstance("event:/StoneElevator");
        instanceElevator.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(start.transform.position));
    }
    private void Update()
    {
        if (isDefault)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, start.transform.position, speed);
        }
        if (isDefault == false)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, end.transform.position, speed);
        }
    }

    void Activate()
    {
        StartCoroutine(Raise());
    }

    void Deactivate()
    {
        StartCoroutine(Lower());
    }
    IEnumerator Raise()
    {
        instanceElevator.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instanceElevator.start();
        isDefault = false;
        yield return new WaitForSeconds(3);
    }

    IEnumerator Lower()
    {
        instanceElevator.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instanceElevator.start();
        isDefault = true;
        yield return new WaitForSeconds(3);
    }
}
