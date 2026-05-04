using UnityEngine;

public class WorldPosition
{
    public static Vector3 GetWorldPosition(Vector3 mousePosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        return worldPosition;
    }
}
