using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SheepTitleScreenButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action OnPointerEnterSheepTitleScreenButton;

    [SerializeField] Vector2 movePosition;
    [SerializeField] float delayBeforeMove;
    [SerializeField] float transparencyAfterBlur;
    float blurTime = 1f;


    Image buttonImage;
    UIPanelMove uIPanelMove;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        uIPanelMove = this.GetComponent<UIPanelMove>();
    }
    private void OnEnable()
    {
        GameManager.OnStartGame += DeactivateButton;
        GameManager.OnStartGame += MoveSheep;
        GameManager.OnStartGame += BlurSheep;
    }

    private void OnDisable()
    {
        GameManager.OnStartGame -= DeactivateButton;
        GameManager.OnStartGame -= MoveSheep;
        GameManager.OnStartGame -= BlurSheep;


    }

    private void Start()
    {
        buttonImage.alphaHitTestMinimumThreshold = 0.1f;

    }



    void DeactivateButton()
    {
        this.GetComponent<Button>().interactable = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(this.GetComponent<Button>().interactable)
        {
            OnPointerEnterSheepTitleScreenButton?.Invoke();
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }


    void MoveSheep()
    { 

        if(uIPanelMove!=null)
        {
            StartCoroutine(MoveSheepRoutine());
        }
    }

    IEnumerator MoveSheepRoutine()
    {
        yield return new WaitForSeconds(delayBeforeMove);

        uIPanelMove.MoveImageTo(movePosition);
    }


    void BlurSheep()
    {

        StartCoroutine(BlurSheepRoutine());
    }

    IEnumerator BlurSheepRoutine()
    {

        float timePassed = 0f;

        while (timePassed < blurTime)
        {
            float t = Mathf.Clamp01(timePassed / blurTime);

            buttonImage.color =  Color.Lerp(new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1f), new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, transparencyAfterBlur), t);

            timePassed += Time.deltaTime;

            yield return null;
        }

        buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, transparencyAfterBlur);


    }
}
