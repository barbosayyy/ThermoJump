using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateObject : MonoBehaviour
{
    public bool isDefault;
    public GameObject currentPos;
    public GameObject nextPos;
    private Vector3 _y = new Vector3(0, 3, 0);

    void Start()
    {
        isDefault = true;
        currentPos = new GameObject();
        nextPos = new GameObject();

        currentPos.transform.position = gameObject.transform.position;
        nextPos.transform.position = currentPos.transform.position;
        nextPos.transform.position += _y;
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
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, nextPos.transform.position, 1f);
        yield return new WaitForSeconds(3);
    }

    IEnumerator Lower()
    {
        isDefault = true;
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, currentPos.transform.position, 1f);
        yield return new WaitForSeconds(3);
    }
}
