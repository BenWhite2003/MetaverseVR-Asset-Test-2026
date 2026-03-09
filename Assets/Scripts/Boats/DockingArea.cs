using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum for organising what state the docking process is in
public enum DockingState
{
    None,
    WrongSide,
    Portside,
    DockingComplete
}
public class DockingArea : MonoBehaviour
{
    // Events for updating the HUD
    public static event Action<DockingState> OnDockStateChanged;
    public static event Action<float> OnTimeLeftUpdated;

    // Bool to keep track of docking status
    private bool playerIsDocked = false;

    // Timer for the powerboat being in the dock area
    private float timeInDock = 0;

    // Stores the amount of time left in the dock
    private float timeLeftInDock;

    // The maximum amount of time the powerboat needs to be docked before mission completion
    [SerializeField] private float maxTimeInDock = 5f;


    // Variable to store the docked boat, this will be used for a portside check
    private GameObject dockedBoat;

    public bool IsDockingComplete = false;

    // Stores the docking state
    private DockingState currentState = DockingState.None;

   
    void Update()
    {
        CheckDockingStatus();
    }

    private bool IsBoatPortside(GameObject boat)
    {
        // Checks if the power boat is portside by looking at its rotation with a buffer of 20f
        // Its 180f becuase in this case that's portside to the dock
        if (boat.transform.eulerAngles.y < 180f - 20f || boat.transform.eulerAngles.y > 180f + 20f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void CheckDockingStatus()
    {
        if (playerIsDocked && dockedBoat != null)
        {
            if (!IsDockingComplete && IsBoatPortside(dockedBoat))
            {
                // Sets the docking state to Portside
                SetDockState(DockingState.Portside);

                // Store time in the dock
                timeInDock += Time.deltaTime;

                // Calculate how much more time is needed in the dock
                timeLeftInDock = maxTimeInDock - timeInDock;

                // Invokes an event to show how much time is left in the dock
                OnTimeLeftUpdated?.Invoke(timeLeftInDock);

                // Docking complete
                if (timeInDock >= maxTimeInDock)
                {
                    IsDockingComplete = true;
                    SetDockState(DockingState.DockingComplete);
                }
            }
            else
            {
                if (!IsDockingComplete)
                {
                    // Set the docking state to "WrongSide", this will tell the player that they need to reposition
                    SetDockState(DockingState.WrongSide);

                    // Resets timers
                    timeInDock = 0f;
                    timeLeftInDock = maxTimeInDock;
                }
            }
        }
        else
        {
            if (!IsDockingComplete)
            {
                SetDockState(DockingState.None);
                timeInDock = 0f;
                timeLeftInDock = maxTimeInDock;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsDocked = true;
            // Sets the docked boat to the boat that has entered the trigger
            dockedBoat = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsDocked = false;

            // Sets the docked boat to be null when the boat exits the trigger
            dockedBoat = null;
        }
    }

    private void SetDockState(DockingState newState)
    {
        // Only switch states if "newState" is a different state
        if (currentState == newState) return;

        // Sets the docking state
        currentState = newState;
        // Invokes an event to show what state the docking process is in
        OnDockStateChanged?.Invoke(newState);
    }
}
