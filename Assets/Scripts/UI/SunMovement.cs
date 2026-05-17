using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SunMovement : MonoBehaviour
{
    [SerializeField] AnimationClip sunMovingClip;
    [SerializeField] AnimationClip sunIdleClip;
    [SerializeField] int numberOfFlashes;
    [SerializeField] Sprite invertedSunSprite;
    Sprite baseSunSprite;

    int SUNIDLE_HASH = Animator.StringToHash("Sun_idle");
    int SUNMOVING_HASH = Animator.StringToHash("Sun_moving");

    Animator animator;
    Image sunImage;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        sunImage = GetComponent<Image>();
    }

    private void Start()
    {
        baseSunSprite = sunImage.sprite;
    }


    public void SetSunSpeed(float timerDurationInSec)
    {
        float multiplier = sunMovingClip.length / timerDurationInSec;
        animator.speed = multiplier;

        //Debug.Log("Multipleier: "+ multiplier);

        ResetSunPosition();
    }

    public void StartSunMovement()
    {
        //Debug.Log("Starting sun movement : "+ SUNMOVING_HASH);
        animator.Play(SUNMOVING_HASH, 0, 0f);
    }
    public void ResetSunPosition()
    {
        //Debug.Log("Resetting sun movement");
        animator.Play(SUNIDLE_HASH, 0, 0f);
    }

    public void StopMovement()
    {
        animator.Play(SUNIDLE_HASH, 0, 0f);
    }

    public void FlashSun()
    {
        StartCoroutine(FlashSunROutine());
    }


    IEnumerator FlashSunROutine()
    {
        int currentNumberOfFlashes = 0 ;
    
        while (currentNumberOfFlashes < numberOfFlashes)
        {
            sunImage.sprite = invertedSunSprite;
            yield return new WaitForSeconds(0.5f);

            currentNumberOfFlashes++;
            sunImage.sprite = baseSunSprite;
            yield return new WaitForSeconds(0.5f);
        }


        sunImage.sprite = baseSunSprite;
    }

}
