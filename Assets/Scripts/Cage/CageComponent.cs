using UnityEngine;

public class CageComponent : MonoBehaviour
{
    [SerializeField] GameObject holeGlowEffect;
    private void OnEnable()
    {
        PointerGrabber.OnGrabbedSheep += ToggleHoleGlowEffect;
    }

    private void OnDisable()
    {
        PointerGrabber.OnGrabbedSheep -= ToggleHoleGlowEffect;

    }



    void ToggleHoleGlowEffect(bool status)
    {
        holeGlowEffect.SetActive(status);
    }
}
