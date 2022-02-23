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
    private Image _horseImage;
    private Image _daggerImage;
    private Image _chaliceImage;

    public TMP_Dropdown resolutionDropdown;
    FMOD.Studio.Bus _master;

    //[SerializeField] [Range(-80f, 10f)]
    //private float masterVolume;

    private Resolution[] _resolutions;

    public Slider sensitivitySlider;
    public Slider volumeSlider;

    private float _t;
    private Color _whiteOpaque;
    private Color _whiteTransparent;
    private Color _horseColor;
    private Color _daggerColor;
    private Color _chaliceColor;
    private Color _horseColorTransparent;
    private Color _daggerColorTransparent;
    private Color _chaliceColorTransparent;
    
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

        _horseImage = _levelManager.horseImage;
        _daggerImage = _levelManager.daggerImage;
        _chaliceImage = _levelManager.chaliceImage;

        _whiteOpaque = new Color(255/255.0f, 241/255.0f, 189/255.0f, 1);
        _whiteTransparent = new Color(255/255.0f, 241/255.0f, 189/255.0f, 0);
        _horseColor = _horseImage.color;
        _daggerColor = _daggerImage.color;
        _chaliceColor = _chaliceImage.color;
        _horseColorTransparent = new Color(_chaliceColor.r,_chaliceColor.g,_chaliceColor.b,0);
        _daggerColorTransparent = new Color(_chaliceColor.r,_chaliceColor.g,_chaliceColor.b,0);
        _chaliceColorTransparent = new Color(_chaliceColor.r,_chaliceColor.g,_chaliceColor.b,0);

        HideScore();
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

        _horseImage.color = Color.Lerp(_horseColor, _horseColorTransparent, _t);
        _chaliceImage.color = Color.Lerp(_chaliceColor, _chaliceColorTransparent, _t);
        _daggerImage.color = Color.Lerp(_daggerColor, _daggerColorTransparent, _t);
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
        _horseImage.color = _horseColor;
        _chaliceImage.color = _chaliceColor;
        _daggerImage.color = _daggerColor;
        horseText.color = _whiteOpaque;
        chaliceText.color = _whiteOpaque;
        daggerText.color = _whiteOpaque;
        StartCoroutine(WaitForFade());
    }

    void HideScore()
    {
        _horseImage.color = _horseColorTransparent;
        _chaliceImage.color = _chaliceColorTransparent;
        _daggerImage.color = _daggerColorTransparent;
        horseText.color = _whiteTransparent;
        chaliceText.color = _whiteTransparent;
        daggerText.color = _whiteTransparent;
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
