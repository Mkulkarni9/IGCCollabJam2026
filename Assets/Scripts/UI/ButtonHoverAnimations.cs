using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonHoverAnimations : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action OnHoverEnter;
    public static event Action OnHoverExit;

    [SerializeField] float animationDuration;
    [Header("Scale settings")]
    [SerializeField] bool changeScale;
    [SerializeField] Vector2 targetScale;
    [SerializeField] AnimationCurve scaleAnimationCurve;

    [Header("Rotation settings")]
    [SerializeField] bool changeRotation;
    [SerializeField] Vector3 targetRotation;
    [SerializeField] AnimationCurve rotationAnimationCurve;

    [Header("Image settings")]
    [SerializeField] bool changeImage;
    [SerializeField] Sprite targetButtonImage;

    [Header("Indicator settings")]
    [SerializeField] bool addIndicator;
    [SerializeField] GameObject indicatorPrefab;
    [SerializeField] Vector3 indicatorOffset;
    
    [Header("Highlight settings")]
    [SerializeField] bool addOuterHighlight;
    [SerializeField] GameObject outerHighlightPrefab;
    [SerializeField] Vector3 outerHighlightOffset;

    [SerializeField] bool addInnerHighlight;
    [SerializeField] GameObject innerHighlightGameobject;

    [Header("Color settings")]
    [SerializeField] bool changeTransparency;

    [Header("Pulse settings")]
    [SerializeField] bool pulseScale;
    [SerializeField] float pulseScaleMax;
    [SerializeField] float pulseScaleMin;
    [SerializeField] float pulseDuration;
    [SerializeField] AnimationCurve scalePulseUpAnimationCurve;
    [SerializeField] AnimationCurve scalePulseDownAnimationCurve;

    [Header("VFX settings")]
    [SerializeField] bool particleVFX;
    [SerializeField] GameObject[] particleVFXPrefabs;



    private Button button;

    Coroutine scaleCoroutine;
    Coroutine resetScaleCoroutine;
    Coroutine rotationCoroutine;
    Coroutine resetRotationCoroutine;
    Coroutine pulseScaleCoroutine;

    Vector2 initialScale;
    Vector3 initialRotation;
    Sprite initialButtonImage;
    GameObject indicator;
    GameObject highlight;


    void Awake()
    {
        button = GetComponent<Button>();
    }

    void Start()
    {
        initialScale = transform.localScale;
        initialRotation = transform.rotation.eulerAngles;
        initialButtonImage = button.image.sprite;

        
    }

    




    #region Pointer event handlers

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable)
        {

            if (changeScale)
            {
                ChangeScale();
            }

            if (changeRotation)
            {
                ChangeRotation();
            }

            if (addIndicator)
            {
                AddIndicators();
            }

            if (addOuterHighlight)
            {
                AddOuterHighlight();
            }

            if (addInnerHighlight)
            {
                AddInnerHighlight();
            }


            if (changeImage)
            {
                ChangeImage();
            }

            if (pulseScale)
            {
                PulseScaleEffect();
            }

            if (particleVFX)
            {
                PlayParticleVFX();
            }

            if (changeTransparency)
            {
                ChangeColor();
            }

            

            OnHoverEnter?.Invoke();
            
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        if (button.interactable)
        {
            if (changeScale)
            {
                ResetScale();
            }

            if (changeRotation)
            {
                ResetRotation();
            }

            if (changeImage)
            {
                ResetImage();
            }

            if (addIndicator)
            {
                RemoveIndicators();
            }

            if (addOuterHighlight)
            {
                RemoveOuterHighlight();
            }

            if (addInnerHighlight)
            {
                RemoveInnerHighlight();
            }

            if (pulseScale)
            {
                StopPulseScaleEffect();
            }

            if (particleVFX)
            {
                StopParticleVFX();
            }

            

            if (changeTransparency)
            {
                ChangeColor();
            }

            OnHoverExit?.Invoke();
        }
    }



    #endregion


    #region scale Animations
    public void ChangeScale()
    {
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
        }

        scaleCoroutine = StartCoroutine(SmoothScaleRoutine());
    }

    private IEnumerator SmoothScaleRoutine()
    {
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            transform.localScale = initialScale + scaleAnimationCurve.Evaluate(elapsed / animationDuration) * (targetScale - initialScale);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
        
        
    }
    
    public void ResetScale()
    {
        if (resetScaleCoroutine != null)
        {
            StopCoroutine(resetScaleCoroutine);
        }

        resetScaleCoroutine = StartCoroutine(ResetScaleRoutine());
    }

    private IEnumerator ResetScaleRoutine()
    {
        float elapsed = 0f;
        Vector2 currentScale = transform.localScale;
        while (elapsed < animationDuration)
        {
            transform.localScale = Vector3.Lerp(currentScale, initialScale, elapsed / animationDuration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localScale = initialScale;
    }
    #endregion

    #region Rotation Animations
    public void ChangeRotation()
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }

        rotationCoroutine = StartCoroutine(SmoothRotationRoutine());
    }

    private IEnumerator SmoothRotationRoutine()
    {
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, initialRotation.z + rotationAnimationCurve.Evaluate(elapsed / animationDuration)* (targetRotation.z - initialRotation.z)); ;
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f,0f,targetRotation.z);
    }
    
    public void ResetRotation()
    {
        if (resetRotationCoroutine != null)
        {
            StopCoroutine(resetRotationCoroutine);
        }

        resetRotationCoroutine = StartCoroutine(ResetRotationRoutine());
    }

    private IEnumerator ResetRotationRoutine()
    {
        float elapsed = 0f;
        Vector3 currentRotation = transform.rotation.eulerAngles;
        while (elapsed < animationDuration)
        {
            float t = elapsed / animationDuration;
            float currentZ = Mathf.LerpAngle(currentRotation.z, initialRotation.z, t);
            transform.rotation = Quaternion.Euler(0f, 0f, currentZ);
            
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f,0f,initialRotation.z);
    }

    #endregion

    #region Image Animations

    public void ChangeImage()
    {
        this.button.image.sprite = targetButtonImage;
    }

    public void ResetImage()
    {
        this.button.image.sprite = initialButtonImage;

        /*if (EventSystem.current == null || EventSystem.current.currentSelectedGameObject != this.button.gameObject)
        {
            
        }*/
        
    }

    #endregion

    #region Indicators

    void AddIndicators()
    {
        
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector3 worldPosition = Vector3.zero;

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, rectTransform.position);
        worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, 10f));

        indicator = Instantiate(indicatorPrefab, worldPosition + indicatorOffset, Quaternion.identity);

        
    }

    void RemoveIndicators()
    {
        Destroy(indicator);
    }

    void AddOuterHighlight()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector3 worldPosition = Vector3.zero;

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, rectTransform.position);
        worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, 10f));

        highlight = Instantiate(outerHighlightPrefab, worldPosition + outerHighlightOffset, Quaternion.identity);
    }

    void RemoveOuterHighlight()
    {
        Destroy(highlight);
    }

    void AddInnerHighlight()
    {
        innerHighlightGameobject.SetActive(true);
    }

    void RemoveInnerHighlight()
    {
        innerHighlightGameobject.SetActive(false);
    }

    #endregion

    #region Pulse Effects

    void PulseScaleEffect()
    {
        if (pulseScaleCoroutine != null)
        {
            StopCoroutine(pulseScaleCoroutine);
        }
        pulseScaleCoroutine = StartCoroutine(PulseScaleRoutine());
        
    }

    IEnumerator PulseScaleRoutine()
    {
        float elapsed = 0f;
        bool scalingUp = true;


        while (true)
        {
            float target = scalingUp ? pulseScaleMax : pulseScaleMin;
            float start = scalingUp ? pulseScaleMin : pulseScaleMax;

            while (elapsed < pulseDuration)
            {
                float t = elapsed / pulseDuration;
                float scale = start + (target - start) * (scalingUp ? scalePulseUpAnimationCurve.Evaluate(t) : scalePulseDownAnimationCurve.Evaluate(t));
                transform.localScale = initialScale * scale;

                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            transform.localScale = initialScale * target;
            elapsed = 0f;
            scalingUp = !scalingUp;
        }
    }
    
    void StopPulseScaleEffect()
    {
        if (pulseScaleCoroutine != null)
        {
            
            StopCoroutine(pulseScaleCoroutine);
        }
        
    }


    #endregion

    #region Particle and other Effects

    void PlayParticleVFX()
    {
        foreach (GameObject vfxPrefab in particleVFXPrefabs)
        {

            vfxPrefab.gameObject.SetActive(true);
        }
    }

    void StopParticleVFX()
    {
        foreach (GameObject vfxPrefab in particleVFXPrefabs)
        {
            if (vfxPrefab.gameObject != null)
            {
                vfxPrefab.gameObject.SetActive(false);
            }
            
        }
    }


    #endregion

    #region Color Animations

    public void ChangeColor()
    {
    }

    #endregion
}
