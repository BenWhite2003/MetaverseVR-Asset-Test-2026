using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunseekerMovement : MonoBehaviour
{
    // Sunseeker Rigidbody so we can change the boats velocity
    private Rigidbody sunseekerRB;

    // Keeps track of what bezier curve we're on
    private int currentCurveIndex = 0;

    // Time variable ranging from 0 to 1 for bezier curve interpolation
    private float t = 0f;

    // The value we multiply delta time by to increase t
    // The lower the value (around <1) the slower it travels
    [SerializeField] private float tIncrement = 0.015f;

    private SunseekerCircuit sunseekerCircuit;

    // The current direction of the curve the sunseeker is travelling in
    private Vector3 currentCurveDirection;
    // The direction of the next curve the sunseeker will travel
    private Vector3 nextCurveDirection;

    void Start()
    {
        // Get the Sunseeker Script to access circuit data
        sunseekerCircuit = GetComponent<SunseekerCircuit>();

        // Get the Rigidbody from the Sunseeker
        sunseekerRB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Create a bezier curve using an array of bezier curves, the current index, and the start, control and end points
        Vector3 bezierCurve = CalculateBezierCurve(sunseekerCircuit.circuit[currentCurveIndex].startPoint.position, sunseekerCircuit.circuit[currentCurveIndex].controlPoint.position, sunseekerCircuit.circuit[currentCurveIndex].endPoint.position, t);

        // Create a direction using the bezier curve (using the boats y transform to ensure we stay at the correct y level)
        Vector3 direction = new Vector3(bezierCurve.x, transform.position.y, bezierCurve.z);

        // Apply the direction (minus the boats position) to the velocity to move the boat through the curve
        sunseekerRB.velocity = (direction - transform.position);

        // Slowly increase time variable (t) based on deltaTime and t increment
        t += Time.deltaTime * tIncrement;

        // Resets time back to 0 if it exceeds 1 to go back to the start of the next bezier curve
        if (t > 1f)
        {
            t = 0f;
            // Moves onto the next bezier curve in the circuit when we reach the end of the current one
            currentCurveIndex++;
        }

        if (currentCurveIndex >= sunseekerCircuit.circuit.Length)
        {
            // Reset the curve index back to 0 when it reaches the end of the array to ensure they loop
            currentCurveIndex = 0;
        }

        // Sets the next curve direction to the first one when we are 1 curve away from the end
        if (currentCurveIndex == sunseekerCircuit.circuit.Length - 1)
        {
            nextCurveDirection = -(CalculateBezierCurve(sunseekerCircuit.circuit[0].startPoint.position, sunseekerCircuit.circuit[0].controlPoint.position, sunseekerCircuit.circuit[0].endPoint.position, t + 0.01f) - transform.position).normalized;
        }
        else
        {
            // Sets the next curve direction by using the current curve index plus 1
            nextCurveDirection = -(CalculateBezierCurve(sunseekerCircuit.circuit[currentCurveIndex + 1].startPoint.position, sunseekerCircuit.circuit[currentCurveIndex + 1].controlPoint.position, sunseekerCircuit.circuit[currentCurveIndex + 1].endPoint.position, t + 0.01f) - transform.position).normalized;
        }


        // Calculate the current direction to look at (minus becuase it was initially facing the opposite way)
        currentCurveDirection = -(CalculateBezierCurve(sunseekerCircuit.circuit[currentCurveIndex].startPoint.position, sunseekerCircuit.circuit[currentCurveIndex].controlPoint.position, sunseekerCircuit.circuit[currentCurveIndex].endPoint.position, t + 0.01f) - transform.position).normalized;


        // Rotate the boat in the desired direction
        RotateBoat(currentCurveDirection, nextCurveDirection, t);
    }

    private Vector3 CalculateBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        // Equation to calculate the bezier curve the boat will be following
        Vector3 b = (1 - t) * (1 - t) * p0 + 2 * (1 - t) * t * p1 + t * t * p2;

        return b;
    }

    // Method to rotate the boat smoothly towards the target direction
    private void RotateBoat(Vector3 currentCurve, Vector3 nextCurve, float t)
    {
        // Calculate the angle to rotate the boat based on the current curve's direction using Atan2
        float angle = Mathf.Atan2(currentCurve.x, currentCurve.z) * Mathf.Rad2Deg;

        // Create a quaternion rotation around the Y-axis only
        Quaternion targetRotation = Quaternion.Euler(0, angle, 0);

        // Calculate the angle for the next curve's direction using Atan2 for smooth transition
        float nextCurveAngle = Mathf.Atan2(nextCurve.x, nextCurve.z) * Mathf.Rad2Deg;

        // Create a quaternion rotation for the next curve's direction
        Quaternion nextTargetRotation = Quaternion.Euler(0, nextCurveAngle, 0);

        // If we're early in the curve (t < 0.5), rotate smoothly towards the target rotation
        if (t < 0.5f)
        {
            // Smoothly rotate towards the current curve's target rotation 
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5.0f);
        }
        else
        {
            // If we're getting close to the end of the curve (t >= 0.5), blend the rotation towards the next curve
            // Smoothly blend the rotation between the current curve's rotation and the next curve's rotation
            // Blend factor increases as t gets closer to 1
            // The bigger the first number (0.9f) the smoother the transition is
            float blendFactor = Mathf.SmoothStep(0, 1, (t - 0.9f) / 0.1f);

            // Apply the blended rotation
            Quaternion blendedRotation = Quaternion.Slerp(targetRotation, nextTargetRotation, blendFactor * 0.1f);
            transform.rotation = blendedRotation;
        }


    }
}
