using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionCompletionManager : MonoBehaviour
{
    [SerializeField] private float missionCompleteDelay = 1f;

    private void OnEnable()
    {
        HUDController.OnMissionComplete += MissionComplete;
    }

    private void OnDisable()
    {
        HUDController.OnMissionComplete -= MissionComplete;
    }

    private void MissionComplete()
    {
        StartCoroutine(PauseGame());
    }

    private IEnumerator PauseGame()
    {
        yield return new WaitForSeconds(missionCompleteDelay);

        Time.timeScale = 0f;
    }
}
