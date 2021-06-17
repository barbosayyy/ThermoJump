using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiGameplay : MonoBehaviour
{
    public GameObject pauseUi;
    private GameObject gameplayUi;
    private Player playerScript;
    private ShootRocket sR;
    private MouseLook lookScript;
    private bool isPaused;

    public GameObject pauseMain;
    public GameObject optionsMain;
    public GameObject bindingsMain;
    public GameObject fadeInCanvas;

    public TMPro.TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    public Slider sensitivitySlider;

    void Start()
    {
        gameplayUi = GameObject.FindGameObjectWithTag("GameplayUI");
        pauseUi = GameObject.FindGameObjectWithTag("PauseUI");
        sR = gameObject.GetComponent<ShootRocket>();
        sR.isPaused = false;
        lookScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>();
        lookScript.mouseSensitivity = sensitivitySlider.value;
        playerScript = gameObject.GetComponent<Player>();
        pauseUi.SetActive(false);
        isPaused = false;

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRate;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void ShowPause()
    {
        if (isPaused != true)
        {
            gameplayUi.SetActive(false);
            pauseUi.SetActive(true);
            sR.isPaused = true;
            Time.timeScale = 0;
            lookScript.canMoveCamera = false;
            playerScript.instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            playerScript.canMove = false;
            Cursor.lockState = CursorLockMode.None;
            isPaused = true;
        }
        else
        {
            sR.isPaused = false;
            gameplayUi.SetActive(true);
            pauseUi.SetActive(false);
            Time.timeScale = 1;
            lookScript.canMoveCamera = true;
            playerScript.canMove = true;
            Cursor.lockState = CursorLockMode.Locked;
            isPaused = false;
        }
    }

    public void HandleInputData(int val)
    {
        switch (val)
        {
            case 0:
                Screen.fullScreen = true;
                Debug.Log("fs");
                break;
            case 1:
                Screen.fullScreen = false;
                Debug.Log("fsnn");
                break;
        }
    }

    public void HandleSensitivity()
    {
        lookScript.mouseSensitivity = sensitivitySlider.value;
    }

    public void ShowOptionsMain()
    {
        pauseMain.SetActive(false);
        bindingsMain.SetActive(false);
        optionsMain.SetActive(true);

    }

    public void ShowPauseMain()
    {
        pauseMain.SetActive(true);
        optionsMain.SetActive(false);
    }

    public void ShowBindingsMain()
    {
        optionsMain.SetActive(false);
        bindingsMain.SetActive(true);
    }

    public void Resume()
    {
        sR.isPaused = false;
        gameplayUi.SetActive(true);
        pauseUi.SetActive(false);
        Time.timeScale = 1;
        lookScript.canMoveCamera = true;
        playerScript.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }
    
    public void QuitToMenu()
    {
        StartCoroutine(Quit());
    }

    IEnumerator Quit()
    {
        Time.timeScale = 1;
        fadeInCanvas.SetActive(true);
        SceneManager.LoadScene(1);
        yield break;
    }
}
