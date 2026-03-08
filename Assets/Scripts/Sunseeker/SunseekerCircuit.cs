using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // Allows changing the variable in editor

// Struct that holds the start point, control point and end point for a bezier curve
public struct BezierCurve
{
    public Transform startPoint, controlPoint, endPoint;
}
public class SunseekerCircuit : MonoBehaviour
{
    // Array of Bezier curves that represent the circuit path
    public BezierCurve[] circuit;
}
