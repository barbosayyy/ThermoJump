using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslatePlatform : MonoBehaviour
{
    public bool isDefault;
    public GameObject start;
    public GameObject end;
    public float speed;

    private void Start()
    {
        isDefault = true;
        gameObject.transform.position = start.transform.position;
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
        yield return new WaitForSeconds(3);
    }

    IEnumerator Lower()
    {
        isDefault = true;
        yield return new WaitForSeconds(3);
    }
}
