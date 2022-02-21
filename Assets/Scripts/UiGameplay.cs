using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiGameplay : MonoBehaviour
{
    public GameObject pauseUi;
    private GameObject _gameplayUi;
    private Player _playerScript;
    private ShootRocket _sR;
    private MouseLook _lookScript;
    private LevelManager _levelManager;
    private bool _isPaused;
    private bool _fade;

    public GameObject pauseMain;
    public GameObject optionsMain;
    public GameObject bindingsMain;
    public GameObject fadeInCanvas;
    public TMP_Text horseText;
    public TMP_Text daggerText;
    public TMP_Text chaliceText;
    public TMP_Text horseTextChild;
    public TMP_Text chaliceTextChild;
    public TMP_Text daggerTextChild;

    public TMP_Dropdown resolutionDropdown;
    FMOD.Studio.Bus _master;

    //[SerializeField] [Range(-80f, 10f)]
    //private float masterVolume;

    private Resolution[] _resolutions;

    public Slider sensitivitySlider;
    public Slider volumeSlider;

    private Color _whiteOpaque;
    private Color _whiteTransparent;
    private float _t;

    void Start()
    {
        _gameplayUi = GameObject.FindGameObjectWithTag("GameplayUI");
        pauseUi = GameObject.FindGameObjectWithTag("PauseUI");
        _sR = gameObject.GetComponent<ShootRocket>();
        _sR.isPaused = false;
        _lookScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>();
        _lookScript.mouseSensitivity = sensitivitySlider.value;
        _playerScript = gameObject.GetComponent<Player>();
        pauseUi.SetActive(false);
        _isPaused = false;

        _resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for(int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height + " @ " + _resolutions[i].refreshRate;
            options.Add(option);

            if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        
        _master = FMODUnity.RuntimeManager.GetBus("bus:/Master");

        volumeSlider.minValue = 0;
        volumeSlider.maxValue = 1;
        _levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        _levelManager.onScoreChanged.AddListener(ShowScore);
        _whiteOpaque = new Color(1, 1, 1, 1);
        _whiteTransparent = new Color(1, 1, 1, 0);
    }

    private void Update()
    {
        if (_fade)
        {
            LerpAlpha();
        }
        else
        {
            
        }
    }

    public void ShowPause()
    {
        if (_isPaused != true)
        {
            _gameplayUi.SetActive(false);
            pauseUi.SetActive(true);
            _sR.isPaused = true;
            Time.timeScale = 0;
            _lookScript.canMoveCamera = false;
            _playerScript.instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _playerScript.canMove = false;
            Cursor.lockState = CursorLockMode.None;
            _isPaused = true;
        }
        else
        {
            _sR.isPaused = false;
            _gameplayUi.SetActive(true);
            pauseUi.SetActive(false);
            Time.timeScale = 1;
            _lookScript.canMoveCamera = true;
            _playerScript.canMove = true;
            Cursor.lockState = CursorLockMode.Locked;
            _isPaused = false;
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
        _lookScript.mouseSensitivity = sensitivitySlider.value;
    }

    public void HandleVolume()
    {
        _master.setVolume(volumeSlider.value);
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
        _sR.isPaused = false;
        _gameplayUi.SetActive(true);
        pauseUi.SetActive(false);
        Time.timeScale = 1;
        _lookScript.canMoveCamera = true;
        _playerScript.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        _isPaused = false;
    }
    
    public void QuitToMenu()
    {
        _playerScript.instanceForestSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        StartCoroutine(Quit());
    }

    void LerpAlpha()
    {
        _t += Time.deltaTime / 3f;
        horseTextChild.color = Color.Lerp(_whiteOpaque, _whiteTransparent, _t);
        chaliceTextChild.color = Color.Lerp(_whiteOpaque, _whiteTransparent, _t);
        daggerTextChild.color = Color.Lerp(_whiteOpaque, _whiteTransparent, _t);
        horseText.color = Color.Lerp(_whiteOpaque, _whiteTransparent, _t);
        chaliceText.color = Color.Lerp(_whiteOpaque, _whiteTransparent, _t);
        daggerText.color = Color.Lerp(_whiteOpaque, _whiteTransparent, _t);

        if (horseText.color.a <= 0 && chaliceText.color.a <= 0 && daggerText.color.a <= 0)
        {
            _fade = false;
        }
    }

    public void ShowScore()
    {
        horseText.color = _whiteOpaque;
        daggerText.color = _whiteOpaque;
        chaliceText.color = _whiteOpaque;
        horseTextChild.color =_whiteOpaque;
        daggerTextChild.color = _whiteOpaque;
        chaliceTextChild.color = _whiteOpaque;
        StartCoroutine(WaitForFade());
    }

    IEnumerator Quit()
    {
        Time.timeScale = 1;
        fadeInCanvas.SetActive(true);
        SceneManager.LoadScene(0);
        yield break;
    }

    IEnumerator WaitForFade()
    {
        yield return new WaitForSeconds(3);
        yield return _fade = true;
    }

    //private float DecibelToLinear(float db)
    //{
    //    float linear = Mathf.Pow(10.0f, db / 20f);
    //    return linear;
    //}
}
