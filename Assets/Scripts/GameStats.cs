using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStats : MonoBehaviour
{
    public TextMeshProUGUI fps;

    void Start()
    {
        fps = gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        fps.text = "FPS: " + (int)(1f / Time.unscaledDeltaTime);
    }
}
