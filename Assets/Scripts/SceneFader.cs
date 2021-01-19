using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private AnimationCurve scndCurve;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, t);
            yield return 0;
        }
    }

    public IEnumerator FadeOut()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            float a = scndCurve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, t);
            yield return 0;
        }
    }
}
