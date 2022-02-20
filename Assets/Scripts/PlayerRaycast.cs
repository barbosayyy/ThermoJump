using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

// Script for the Player's Raycast on objects

public class PlayerRaycast : MonoBehaviour
{
    public GameObject raycastOrigin;
    public LayerMask interactablesMask;
    public float requiredDistance;
    public GameObject EInteract;
    public Interactable interactObjScript;

    private void Update()
    {
            if (interactObjScript == null)
            {
                interactObjScript = GameObject.FindGameObjectWithTag("Interact").GetComponent<Interactable>();
            }
    }

    void FixedUpdate()
    {
        Ray ray= Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            try
            {
                if (Vector3.Distance(gameObject.transform.position, hit.collider.gameObject.transform.position) < requiredDistance && hit.collider.CompareTag("Interact"))
                {
                    EInteract.SetActive(true);
                    interactObjScript = hit.collider.GetComponent<Interactable>();
                    interactObjScript.isLooking = true;
                }
                else
                {
                    EInteract.SetActive(false);
                    interactObjScript.isLooking = false;
                    interactObjScript = null;
                }
            }
            catch (NullReferenceException exception)
            {
                 
            }
        }

    }
}