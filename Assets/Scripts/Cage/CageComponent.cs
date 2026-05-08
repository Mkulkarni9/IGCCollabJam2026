using UnityEngine;

public class CageComponent : MonoBehaviour
{
    [SerializeField] GameObject holeGlowEffect;
    [SerializeField] ParticleSystem correctCageVFX;
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

    public void DestroyCage()
    {
        Cage cage = GetComponentInParent<Cage>();

        if(cage!=null)
        {
            cage.DestroyCage();
        }
        
    }

    public void PlayCorrectCageVFX()
    {
        correctCageVFX.Play();
    }
}
