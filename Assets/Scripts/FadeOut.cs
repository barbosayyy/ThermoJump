using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FadeOut : MonoBehaviour
{
    public Image blackFade;
    public float seconds;
    Coroutine coroutine;
    public UnityEvent beforeFadeEvent;
    private float t = 0;
    private float _startValue;

    public void Start()
    {
        blackFade.canvasRenderer.SetAlpha(1f);

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(fade());
    }

    IEnumerator fade()
    {
        t = 0;
        _startValue = blackFade.color.a;
        while (t < seconds)
        {
            t += Time.deltaTime;
            blackFade.canvasRenderer.SetAlpha(Mathf.Lerp(_startValue, 0f, t/seconds));
            yield return null;
        }

        if (t >= seconds)
        {
            beforeFadeEvent.Invoke();
            gameObject.SetActive(false);
        }
    }
}
