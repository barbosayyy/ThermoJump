using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool isInRange;
    public UnityEvent interactAction;

    private void Start()
    {
        isInRange = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") || collider.CompareTag("Rocket"))
        {
            isInRange = true;
            interactAction.Invoke();
        }
        else
        {
            isInRange = false;
        }
    }
}
