using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public LevelManager levelmanager;

    public int checkpointId;

    public GameObject Player;
    private void Start()
    {
        levelmanager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CheckpointChanger();
        }
    }

    void CheckpointChanger()
    {
        if (checkpointId == 0)
            {
                levelmanager.playerCheckpoint = 1;
            }

            if (checkpointId == 1)
            {
                levelmanager.playerCheckpoint = 2;
            }

            if (checkpointId == 2)
            {
                levelmanager.playerCheckpoint = 3;
            }
    }
}
