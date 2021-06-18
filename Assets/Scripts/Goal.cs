using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public LevelManager levelManager;
    public float rotationsPerMinute;

    public float treasureId;

    // 0 Horse
    // 1 Dagger
    // 2 Chalice

    public FMOD.Studio.EventInstance instanceTreasureFound;

    void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();

        instanceTreasureFound = FMODUnity.RuntimeManager.CreateInstance("event:/TreasureFound");
    }

    private void Update()
    {
        if (treasureId == 1 || treasureId == 2)
        {
            transform.Rotate(0, 0, 6.0f * rotationsPerMinute * Time.deltaTime);
        }
        else if (treasureId == 0)
        {
            transform.Rotate(0, 6.0f * rotationsPerMinute * Time.deltaTime, 0);
        }
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (treasureId)
            {
                case 0:
                    levelManager.collectible0Score++;
                    levelManager.col0.text = levelManager.collectible0Score + "/" + "3";
                    instanceTreasureFound.start();
                    break;
                case 1:
                    levelManager.collectible1Score++;
                    levelManager.col1.text = levelManager.collectible1Score + "/" + "3";
                    instanceTreasureFound.start();
                    break;
                case 2:
                    levelManager.collectible2Score++;
                    levelManager.col2.text = levelManager.collectible2Score + "/" + "3";
                    instanceTreasureFound.start();
                    break;
                case 3:
                    SceneManager.LoadScene(1);
                    break;

            }
            gameObject.SetActive(false);
        }
    }
}
