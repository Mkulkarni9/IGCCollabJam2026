using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    InputSystem_Actions inputActions;

    InputAction grab;
    InputAction release;
    
    

    public FrameInput FrameInput { get; private set; }


    private void Awake()
    {
        inputActions = new InputSystem_Actions();

        grab = inputActions.Player.Grab;
        release = inputActions.Player.Release;


    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        FrameInput = GatherInput();
    }

    public FrameInput GatherInput()
    {
        return new FrameInput
        {

            Grab = grab.WasPressedThisFrame(),
            Release = release.WasReleasedThisFrame(),

        };
    }

}

public struct FrameInput
{
    public bool Grab;
    public bool Release;
    

    

}
