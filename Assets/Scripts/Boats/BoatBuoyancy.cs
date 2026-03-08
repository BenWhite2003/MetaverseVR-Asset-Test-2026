using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBuoyancy : MonoBehaviour
{
    // Controls the height of the bobbing motion
    [SerializeField] private float amplitude = 0.2f;

    // Controls the speed of the bobbing motion
    [SerializeField]  private float frequency = 1.5f;

    void Update()
    {
        // Calculate a sine wave value to simulate an up and down motion
        float sineWave = amplitude * (Mathf.Sin(frequency * Time.time));

        // Apply the sine wave value as a vertical offset over time
        // Time.deltaTime ensures smooth movement based on frame rate
        transform.position += new Vector3(0, sineWave * Time.deltaTime, 0);
    }
}
