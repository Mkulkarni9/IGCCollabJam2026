using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour
{


    [SerializeField] float flashDurationUp;
    [SerializeField] float flashDurationDown;
    [SerializeField]Color flashColor;

    Coroutine flashRedScreenRoutine;


    Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        Cage.OnAnimalCapturedInWrongCage += FlashRedScreen;
        Animal.OnEatenByWolf += FlashRedScreen;


    }

    private void OnDisable()
    {
        Cage.OnAnimalCapturedInWrongCage -= FlashRedScreen;
        Animal.OnEatenByWolf -= FlashRedScreen;



    }


    void FlashRedScreen()
    {
        FlashRedScreen(null, null);
    }


    void FlashRedScreen(Animal animal, Cage cage)
    {
        if(flashRedScreenRoutine!=null)
        {
            StopCoroutine(flashRedScreenRoutine);
        }

        flashRedScreenRoutine = StartCoroutine(FlashRedScreenRoutine());
    }


    IEnumerator FlashRedScreenRoutine()
    {
        float timePassed = 0f;


        while (timePassed <= flashDurationUp)
        {
            timePassed += Time.deltaTime;
            float alpha = (timePassed / flashDurationUp) * flashColor.a;

            image.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);

            yield return null;
        }

        timePassed = flashDurationDown;
        image.color = flashColor;



        while (timePassed >= 0f)
        {
            timePassed -= Time.deltaTime;
            float alpha = (timePassed / flashDurationDown) * flashColor.a;

            image.color = new Color(flashColor.r, flashColor.g, flashColor.b,alpha);

            yield return null;
        }

        timePassed = 0f;
        image.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0f);
    }
}
