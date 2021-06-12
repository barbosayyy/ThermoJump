using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiGameplay : MonoBehaviour
{
    public GameObject pauseUi;
    private GameObject gameplayUi;
    private Player playerScript;
    private MouseLook lookScript;
    private bool isPaused;
    void Start()
    {
        gameplayUi = GameObject.FindGameObjectWithTag("GameplayUI");
        pauseUi = GameObject.FindGameObjectWithTag("PauseUI");
        lookScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>();
        playerScript = gameObject.GetComponent<Player>();
        pauseUi.SetActive(false);
        isPaused = false;
    }

    public void ShowPause()
    {
        if (isPaused != true)
        {
            gameplayUi.SetActive(false);
            pauseUi.SetActive(true);
            Time.timeScale = 0;
            lookScript.canMoveCamera = false;
            playerScript.instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            playerScript.canMove = false;
            Cursor.lockState = CursorLockMode.None;
            isPaused = true;
        }
        else
        {
            gameplayUi.SetActive(true);
            pauseUi.SetActive(false);
            Time.timeScale = 1;
            lookScript.canMoveCamera = true;
            playerScript.canMove = true;
            Cursor.lockState = CursorLockMode.Locked;
            isPaused = false;
        }
    }
}
