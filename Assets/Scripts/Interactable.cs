using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool isInRange;
    public bool isLooking;
    public UnityEvent interactAction;
    public float waitCounter;

    private void Start()
    {
        isInRange = false;
        isLooking = false;
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Player") || collider.CompareTag("Rocket"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player")|| collider.CompareTag("Rocket"))
        {
            isInRange = false;
        }
    }

    public void Interact()
    {
        if (isLooking == true)
        {
            Debug.Log("Interacted");
            interactAction.Invoke();
            StartCoroutine(Count());
        }
    }

    IEnumerator Count()
    {
        yield return new WaitForSeconds(waitCounter);
        Debug.Log("Interacted");    
        interactAction.Invoke();
    }
}
