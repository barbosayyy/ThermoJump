using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    public Scene currentSceneIndex;
    public GameObject fadeInCanvas;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene();
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
        SceneManager.LoadScene(currentSceneIndex.buildIndex);
        yield break;
    }
}
