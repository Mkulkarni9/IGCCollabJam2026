using UnityEngine;

public class SheepHoverHighlight : MonoBehaviour
{

    SpriteRenderer spriteRenderer;
    Color baseColor;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseColor = spriteRenderer.color;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        PointerGrabber pointerGrabber = collision.GetComponent<PointerGrabber>();

        if (pointerGrabber != null)
        {
            if(!pointerGrabber.IsGrabbingAnimal)
            {
                ToggleHighlight(true);
            }
            
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        PointerGrabber pointerGrabber = collision.GetComponent<PointerGrabber>();

        if (pointerGrabber != null)
        {
            ToggleHighlight(false);

        }
    }


    public void ToggleHighlight(bool status)
    {
        if(status)
        {
            spriteRenderer.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1f);
        }
        else
        {
            spriteRenderer.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0f);
        }
        
    }
}
