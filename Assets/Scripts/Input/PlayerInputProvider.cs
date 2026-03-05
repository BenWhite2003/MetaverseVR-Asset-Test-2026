using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputProvider : MonoBehaviour, IPlayerInput
{
    public float ThrottleValue { get; private set; }

    public float SteerValue { get; private set; }

    // Input Action asset to read input from user
    [SerializeField] private InputActionAsset playerInputActionAsset;

    // Create input actions for each action
    private InputAction throttleAction;
    private InputAction steerAction;
    private InputAction toggleReverseAction;

    private void OnEnable() // Enable the action map on enable
    {
        playerInputActionAsset.FindActionMap("Player Action Map").Enable();
    }

    private void OnDisable() // Disable the action map on disable
    {
        playerInputActionAsset.FindActionMap("Player Action Map").Disable();
    }
    private void Awake()
    {
        // On Awake find the action map "PlayerControls" and enable it
        playerInputActionAsset.FindActionMap("Player Action Map").Enable();

        // Find the input actions and set them
        throttleAction = playerInputActionAsset.FindAction("Throttle");
        steerAction = playerInputActionAsset.FindAction("Steer");
        toggleReverseAction = playerInputActionAsset.FindAction("Toggle Reverse");
    }

    // Update is called once per frame
    private void Update()
    {
        // Read values from the input actions
        ThrottleValue = throttleAction.ReadValue<float>();
        SteerValue = steerAction.ReadValue<float>();
    }

    public bool ToggleReverse()
    {
        return toggleReverseAction.WasPressedThisFrame();
    }
}
