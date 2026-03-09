using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerboatMovement : MonoBehaviour
{
    // Movement events
    public static event Action<float> OnSpeedChanged;
    public static event Action<bool> OnReverseToggled;

    private IPlayerInput input;
    private Rigidbody powerboatRB;
    

    // Movement variables
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float maxReverseSpeed = 5f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float deceleration = 1f;

    public float currentSpeed;
    // Toggled by pressing R
    private bool isReversing = false;

    // The direction the boat is moving in (world space)
    private Vector3 moveDirection;

    [SerializeField] private float turnSpeed = 30f;
    private float turnInput;

    // Used by the turning method to ensure we have some turning even at high speeds
    [SerializeField] private float minTurnFactor = 0.3f;

    private void Awake()
    {
        // Get the Rigidbody component of the powerboat
        powerboatRB = GetComponent<Rigidbody>();

        // Gets the interface from the PlayerInputProvider
        // The PlayerInputProvider must be on the same game object as this script
        input = GetComponent<IPlayerInput>();
    }

    private void Update()
    {
        ReadInput();
        SteerBoat();
    }

    private void FixedUpdate()
    {
        MoveBoat();
    }

    private void Accelerate()
    {
        // Check if the boat is in reverse gear
        if (!isReversing)
        {
            // Gradually increase the current speed towards max speed based on acceleration
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            // Gradually increase the current speed towards max reverse speed based on acceleration
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxReverseSpeed, acceleration * Time.deltaTime);
        }
    }

    private void Decelerate()
    {
        // Gradually decrease the current speed towards 0 based on deceleration
        currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
    }

    private void MoveBoat()
    {
        // Move the boat forward or backward based on current speed and whether its in reverse
        if (!isReversing)
        {
            // Forward
            moveDirection = transform.forward * currentSpeed;
        }
        else
        {
            // Backward
            moveDirection = -transform.forward * currentSpeed;
        }
        // Apply the new velocity while maintaining the existing y velocity
        powerboatRB.velocity = new Vector3(moveDirection.x, powerboatRB.velocity.y, moveDirection.z);

        OnSpeedChanged?.Invoke(powerboatRB.velocity.magnitude);
    }
    private void SteerBoat()
    {
        // Checks if we are getting steering input and ensures the boat is moving before allowing steering
        if (input.SteerValue != 0 && currentSpeed >= 1f)
        {
            // Turning is stronger at low speeds and weaker at high speeds
            float speedFactor = Mathf.Max(1 - (currentSpeed / maxSpeed), minTurnFactor);

            // Adjust turn speed based on speed factor
            float adjustedTurnSpeed = turnSpeed * speedFactor;

            // Calculate turning amount based on input
            float turnAmount = input.SteerValue * adjustedTurnSpeed * Time.deltaTime;

            // Apply rotational force using torque for smooth turning
            powerboatRB.AddTorque(transform.up * turnAmount, ForceMode.Acceleration);
        }
    }

    private void ReadInput()
    {
        // Read throttle input value
        if (input.ThrottleValue > 0)
        {
            // Accelerate if we hold w
            Accelerate();
        }
        else if (input.ThrottleValue < 0)
        {
            // Decelerate if we hold s
            Decelerate();
        }

        // Checks toggle reverse press and if the boat is stationary to avoid switching gears while the boat is moving
        if (input.ToggleReverse() && Mathf.Abs(currentSpeed) == 0)
        {
            // Toggle reverse gear
            isReversing = !isReversing;
            OnReverseToggled?.Invoke(isReversing);
        }
    }
}
