using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SheepTitleScreenButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action OnPointerEnterSheepTitleScreenButton;

    [SerializeField] Vector2 movePosition;

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
        UIPanelMove uIPanelMove = this.GetComponent<UIPanelMove>();

        if(uIPanelMove!=null)
        {
            uIPanelMove.MoveImageTo(movePosition);
        }
    }
}
