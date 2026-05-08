using UnityEngine;

public class AnimalShadow : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Color baseColor;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseColor = spriteRenderer.color;
    }



    public void ToggleHighlight(bool status)
    {
        if (status)
        {
            spriteRenderer.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0.2f);
        }
        else
        {
            spriteRenderer.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0f);
        }

    }
}
