using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    public Scene currentSceneIndex;
    public LevelManager levelmanager;
    public GameObject player;
    private Guardian _guardian;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene();
        levelmanager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Kill());
        }
        else if (other.CompareTag("Guardian"))
        {
            _guardian = other.GetComponent<Guardian>();
            _guardian.deactivated = true;
            _guardian = null;
        }
    }

    IEnumerator Kill()
    {
        
        switch (levelmanager.playerCheckpoint)
        {
            case 0:
                levelmanager.FadeInCanvas();
                player.transform.position = levelmanager.spawn.position;
                break;
            case 1:
                levelmanager.FadeInCanvas();
                player.transform.position = levelmanager.checkpoint0.position;
                break;
            case 2:
                levelmanager.FadeInCanvas();
                player.transform.position = levelmanager.checkpoint1.position;
                break;
            case 3:
                levelmanager.FadeInCanvas();
                player.transform.position = levelmanager.checkpoint2.position;
                break;
        }
        yield break;
    }
}
