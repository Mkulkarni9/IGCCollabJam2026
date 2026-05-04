using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;

    FrameInput frameInput;

    PointerGrabber pointerGrabber;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        pointerGrabber = GetComponent<PointerGrabber>();
    }


    private void Update()
    {
        GatherInputs();

        Grab();
        Release();
    }

    


    void GatherInputs()
    {
        frameInput = playerInput.FrameInput;
    }


    void Grab()
    {
        if(frameInput.Grab)
        {
            pointerGrabber.GrabAnimal();
            pointerGrabber.GrabCage();
        }
    }

    void Release()
    {
        if (frameInput.Release)
        {
            pointerGrabber.ReleaseAnimal();
            pointerGrabber.ReleaseCage();

        }
    }



   

}
