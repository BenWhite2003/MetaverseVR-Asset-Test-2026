using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour
{
    public static event Action OnMissionComplete;

    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI reverseToggleText;
    [SerializeField] TextMeshProUGUI missionTimeText;


    [SerializeField] GameObject dockedPortsideText;
    [SerializeField] GameObject dockedWrongsideText;
    [SerializeField] TextMeshProUGUI dockingTimeText;


    [SerializeField] GameObject missionCompleteText;

    private void OnEnable()
    {
        PowerboatMovement.OnSpeedChanged += SetSpeedText;
        PowerboatMovement.OnReverseToggled += SetReverseText;
        MissionTimeManager.OnTimeUpdated += SetMissionTimeText;

        DockingArea.OnDockStateChanged += HandleDockingUI;
        DockingArea.OnTimeLeftUpdated += SetDockingTimeText;
    }

    private void OnDisable()
    {
        PowerboatMovement.OnSpeedChanged -= SetSpeedText;
        PowerboatMovement.OnReverseToggled -= SetReverseText;
        MissionTimeManager.OnTimeUpdated -= SetMissionTimeText;

        DockingArea.OnDockStateChanged -= HandleDockingUI;
        DockingArea.OnTimeLeftUpdated -= SetDockingTimeText;
    }

    private void SetSpeedText(float speed)
    {
        float truncatedSpeed = Mathf.Floor(speed * 10f) / 10f;
        speedText.text = truncatedSpeed.ToString();
    }

    private void SetReverseText(bool isReversing)
    {
        if (isReversing)
        {
            // Sets the reverse toggle text to green if the boat is in reverse
            reverseToggleText.color = Color.green;
        }
        else
        {
            // Sets the reverse toggle text to white if the boat is NOT in reverse
            reverseToggleText.color = Color.white;
        }
    }

    private void SetMissionTimeText(float missionTime)
    {
        // Calculate the mission time into hours, minutes, and seconds
        int hours = (int)(missionTime / 3600);
        int minutes = (int)(missionTime % 3600) / 60;
        int seconds = (int)(missionTime % 60);

        // Format time to display as 00:00:00
        missionTimeText.text = $"{hours:00}:{minutes:00}:{seconds:00}";
    }

    private void SetDockingTimeText(float dockingTime)
    {
        // Calculates the dockingTime into seconds
        int seconds = (int)(dockingTime % 60);

        // Only set text if seconds is not below 0
        if (seconds >= 0)
        {
            // Set the docking time text to show docking time left
            dockingTimeText.text = $"Time Till Dock Completion: {seconds}";
        }
    }


    public void ResetGame()
    {
        // Loads the current scene (resets game)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void HandleDockingUI(DockingState dockingState)
    {
        switch (dockingState)
        {
            case DockingState.None:
                // Hide all docking UI
                dockedPortsideText.SetActive(false);
                dockedWrongsideText.SetActive(false);
                missionCompleteText.SetActive(false);
                break;

            case DockingState.Portside:
                // Show portside UI, and hide wrongside UI
                dockedPortsideText.SetActive(true);
                dockedWrongsideText.SetActive(false);
                missionCompleteText.SetActive(false);
                break;

            case DockingState.WrongSide:
                // Show the worngside UI, and hide the portside UI
                dockedWrongsideText.SetActive(true);
                dockedPortsideText.SetActive(false);
                missionCompleteText.SetActive(false);
                break;

            case DockingState.DockingComplete:
                // Show docking conplete UI panel, and hide the others
                dockedPortsideText.SetActive(false);
                dockedWrongsideText.SetActive(false);
                missionCompleteText.SetActive(true);
                

                OnMissionComplete?.Invoke();
                break;
        }
    }
}
