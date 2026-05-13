using System;
using System.Collections;
using UnityEngine;

public class UIPanelMove : MonoBehaviour
{
    public static event Action OnMoveUIPanel;

    [SerializeField] float moveDuration;
    [SerializeField] AnimationCurve moveTrajectory;


    Coroutine moveRoutine;
    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        rectTransform.anchoredPosition = this.rectTransform.anchoredPosition;

    }



    public void MoveImageTo(Vector2 endPosition)
    {
        if (moveRoutine !=null)
        {
            StopCoroutine(moveRoutine);
        }

        moveRoutine = StartCoroutine(MoveImageRoutine(endPosition));
    }


    IEnumerator MoveImageRoutine(Vector2 endPosition)
    {
        OnMoveUIPanel?.Invoke();


        Vector2 start = rectTransform.anchoredPosition;
        float timePassed = 0f;

        while (timePassed < moveDuration)
        {
            timePassed += Time.deltaTime;
            float t = Mathf.Clamp01(timePassed / moveDuration);
            float curveT = moveTrajectory.Evaluate(t);
            rectTransform.anchoredPosition = Vector2.Lerp(start, endPosition, curveT);

            yield return null;
        }

        rectTransform.anchoredPosition = endPosition;


    }
}
