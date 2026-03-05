using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInput 
{
    float ThrottleValue { get; } // Value of the throttle input will be (-1 or 1)
    float SteerValue { get; } // Value of the steer input will be (-1 or 1)
    bool ToggleReverse(); // Method to read the reverse input
}
