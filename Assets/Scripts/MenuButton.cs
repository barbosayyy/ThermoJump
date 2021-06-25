using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public Transform pivot;
    public GameObject ball;
    public float seconds;
    public bool translate;
    public GameObject mainMenu;
    public GameObject creditsMenu;

    public FMOD.Studio.EventInstance instanceButton;

    private void Start()
    {
        instanceButton = FMODUnity.RuntimeManager.CreateInstance("event:/Button");
    }

    public void MouseEnter()
    {
        instanceButton.start();

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(time());
    }

    IEnumerator time()
    {
        float T = 0;
        Vector3 startTransform = ball.transform.position;
        while (T < seconds)
        {
            T += Time.deltaTime;
            ball.transform.position = Vector3.Slerp(startTransform, pivot.transform.position, T/seconds);
            yield return null;
        }
    }

    Coroutine coroutine;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Credits()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void Back()
    {
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
