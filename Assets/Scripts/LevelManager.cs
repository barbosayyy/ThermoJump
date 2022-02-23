using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public int collectible0Score;
    public int collectible1Score;
    public int collectible2Score;

    public int playerCheckpoint;

    public TMPro.TMP_Text col0;
    public TMPro.TMP_Text col1;
    public TMPro.TMP_Text col2;

    public Image horseImage;
    public Image daggerImage;
    public Image chaliceImage;
    
    public Transform spawn;
    public Transform checkpoint0;
    public Transform checkpoint1;
    public Transform checkpoint2;

    public UnityEvent onScoreChanged = new UnityEvent();

    public GameObject fadeInCanvas;

    // 0 Horse
    // 1 Dagger
    // 2 Chalice

    private void Start()
    {
        playerCheckpoint = 0;
        SetScoreText();
    }
    void SetScoreText()
    {      
        col0.text = collectible0Score + "/" + "3";
        col1.text = collectible1Score + "/" + "3";
        col2.text = collectible2Score + "/" + "3";
    }
    
    public void FadeInCanvas()
    {
        //fadeInCanvas.SetActive(true);
    }
}
