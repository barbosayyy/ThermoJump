using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// SCRIPT DEPRECATED, USE RESET.CS INSTEAD

public class GameOver : MonoBehaviour
{
    public Scene currentSceneIndex;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(currentSceneIndex.buildIndex);
        }
    }
}
