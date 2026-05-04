using UnityEngine;
using TMPro;
public class CreateWorldText
{


    public static TextMeshPro CreateTextMesh(string message, Vector3 textPosition, float fontsize )
    {
        GameObject textObject = new GameObject("WorldText", typeof(TextMeshPro));
        textObject.gameObject.transform.position = textPosition;


        TextMeshPro textMesh = textObject.GetComponent<TextMeshPro>();
        textMesh.rectTransform.sizeDelta = new Vector2(1, 1);
        textMesh.text = message;
        textMesh.fontSize = fontsize * 2;
        textMesh.alignment = TextAlignmentOptions.Center;


        return textMesh;
    }
}
