using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleElevator1 : MonoBehaviour
{
    public GameObject pressurePlateObj;
    public PressurePlate pressurePlate;

    private void Start()
    {
        pressurePlate = pressurePlateObj.GetComponent<PressurePlate>();
    }


}
