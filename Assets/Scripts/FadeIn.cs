using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FadeIn : MonoBehaviour
{
    public Image fadeImg;
    public float seconds;
    Coroutine coroutine;
    public UnityEvent fadeEvent;

    private float _t;
    private float _startvalue;

    public void Start()
    {
        fadeImg.canvasRenderer.SetAlpha(0f);
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(fade());
    }

    IEnumerator fade()
    {
        _t = 0;
        _startvalue = fadeImg.color.a;
        while (_t < seconds)
        {
            _t += Time.deltaTime;
            fadeImg.canvasRenderer.SetAlpha(Mathf.Lerp(_startvalue, 1f, _t / seconds));
            yield return null;
        }

        if (_t >= seconds)
        {
            fadeEvent.Invoke();
            gameObject.SetActive(false);
        }
    }
}
