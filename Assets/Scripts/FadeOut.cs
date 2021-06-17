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
        float T = 0;
        float startValue = blackFade.color.a;
        while (T < seconds)
        {
            T += Time.deltaTime;
            blackFade.canvasRenderer.SetAlpha(Mathf.Lerp(startValue, 0f, T/seconds));
            yield return null;
        }

        if (T >= seconds)
        {
            beforeFadeEvent.Invoke();
            gameObject.SetActive(false);
        }
    }
}
