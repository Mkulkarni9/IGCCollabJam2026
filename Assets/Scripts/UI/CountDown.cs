using System.Collections;
using UnityEngine;

public class CountDown : MonoBehaviour
{

    [SerializeField] float scaleUpTime;
    [SerializeField] float scaleDownTime;
    [SerializeField] float minScale;
    [SerializeField] float maxScale;

    Coroutine scaleUpDownRoutine;

    public void ScaleUpDownImage()
    {
        if(scaleUpDownRoutine!=null)
        {
            StopCoroutine(scaleUpDownRoutine);
        }

        scaleUpDownRoutine = StartCoroutine(ScaleUpDownImageRoutine());
    }

    IEnumerator ScaleUpDownImageRoutine()
    {

        float timePassed = 0f;


        while(timePassed< scaleUpTime)
        {
            timePassed += Time.deltaTime;

            float currentScale = (timePassed / scaleUpTime) * maxScale + minScale;


            this.transform.localScale = new Vector3(currentScale, currentScale, 1f);


            yield return null;
        }

        this.transform.localScale = new Vector3(maxScale, maxScale, 1f);
        
        
        
        timePassed = scaleDownTime;

        while (timePassed > 0f)
        {
            timePassed -= Time.deltaTime;

            float currentScale = (timePassed / scaleDownTime) * maxScale + minScale;


            this.transform.localScale = new Vector3(currentScale, currentScale, 1f);

            yield return null;
        }

        this.transform.localScale = new Vector3(minScale, minScale, 1f);

    }
}
