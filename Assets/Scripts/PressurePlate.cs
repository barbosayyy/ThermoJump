using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    public bool isActivated;
    public List<Collider> colliderList;
    public int minWeightCount = 3;
    public string weightTag = "Weight";

    public UnityEvent OnActivated;
    public UnityEvent OnDeactivated;

    private void Start()
    {
        isActivated = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(weightTag))
        {
            return;
        }

        colliderList.Add(other);
        
        if (colliderList.Count >= minWeightCount && !isActivated)
        {
            isActivated = true;
            OnActivated.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(weightTag))
        {
            return;
        }
        colliderList.Remove(other);

        if (colliderList.Count < minWeightCount && isActivated)
        {
            isActivated = false;
            OnDeactivated.Invoke(); 
        }
    }
}
