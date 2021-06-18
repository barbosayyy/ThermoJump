using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslatePlatform : MonoBehaviour
{
    public bool isDefault;
    public GameObject start;
    public GameObject end;
    public float speed;

    public FMOD.Studio.EventInstance instanceStoneSlab;

    private void Start()
    {
        isDefault = true;
        gameObject.transform.position = start.transform.position;

        instanceStoneSlab = FMODUnity.RuntimeManager.CreateInstance("event:/StoneSlab");
        instanceStoneSlab.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(end.transform.position));
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

    public void Interact()
    {
        if (isDefault == true)
        {
            StartCoroutine(Raise());
        }
        else if (isDefault == false)
        {
            StartCoroutine(Lower());
        }
    }

    IEnumerator Raise()
    {
        isDefault = false;
        instanceStoneSlab.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instanceStoneSlab.start();
        yield return new WaitForSeconds(3);
    }

    IEnumerator Lower()
    {
        instanceStoneSlab.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instanceStoneSlab.start();
        isDefault = true;
        yield return new WaitForSeconds(3);
    }
}
