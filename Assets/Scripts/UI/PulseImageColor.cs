using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PulseImageColor : MonoBehaviour
{
    [SerializeField] Color pulseColor;
    [SerializeField] float pulseDurationUp;
    [SerializeField] float pulseDurationDown;

    Coroutine pulseRoutine;
    Image image;
    Color baseColor;

    private void OnEnable()
    {
        ScoreManager.OnScoreUpdated += PulseImage;
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreUpdated -= PulseImage;

    }

    private void Awake()
    {
        image = GetComponent<Image>();
        baseColor = image.color;
    }


    void PulseImage()
    {
        if(pulseRoutine!=null)
        {
            StopCoroutine(pulseRoutine);
            image.color = baseColor;
        }

        pulseRoutine = StartCoroutine(PulseImageRoutine());
    }

    IEnumerator PulseImageRoutine()
    {
        float timePassed = 0f;

        while(timePassed < pulseDurationUp)
        {
            float t = timePassed / pulseDurationUp;

            image.material.SetColor("_GlowColor", Color.Lerp(baseColor, pulseColor, t));

            timePassed += Time.deltaTime;
            yield return null;
        }

        image.material.SetColor("_GlowColor", pulseColor);
        timePassed = 0f;

        while (timePassed < pulseDurationDown)
        {
            float t = timePassed / pulseDurationDown;
            image.material.SetColor("_GlowColor", Color.Lerp(pulseColor, baseColor, t));
            timePassed += Time.deltaTime;
            yield return null;
        }

        image.material.SetColor("_GlowColor", baseColor);

    }
}
