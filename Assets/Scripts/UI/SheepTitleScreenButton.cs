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
    }

    private void OnDisable()
    {
        GameManager.OnStartGame -= DeactivateButton;
        GameManager.OnStartGame -= MoveSheep;

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
        OnPointerEnterSheepTitleScreenButton?.Invoke();
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
}
