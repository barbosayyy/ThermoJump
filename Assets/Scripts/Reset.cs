using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    public Scene currentSceneIndex;
    public GameObject fadeInCanvas;
    public LevelManager levelmanager;
    public GameObject player;

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
    }

    IEnumerator Kill()
    {
        fadeInCanvas.SetActive(true);
        switch (levelmanager.playerCheckpoint)
        {
            case 0:
                fadeInCanvas.SetActive(true);
                player.transform.position = levelmanager.spawn.position;
                break;
            case 1:
                fadeInCanvas.SetActive(true);
                player.transform.position = levelmanager.checkpoint0.position;
                break;
            case 2:
                fadeInCanvas.SetActive(true);
                player.transform.position = levelmanager.checkpoint1.position;
                break;
            case 3:
                fadeInCanvas.SetActive(true);
                player.transform.position = levelmanager.checkpoint2.position;
                break;
        }
        yield break;
    }
}
