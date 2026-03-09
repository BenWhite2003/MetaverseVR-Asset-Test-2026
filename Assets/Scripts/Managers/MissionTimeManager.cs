using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTimeManager : MonoBehaviour
{
    public static event Action<float> OnTimeUpdated;
    private float missionTime;

    // Update is called once per frame
    void Update()
    {
        missionTime += Time.deltaTime;
        OnTimeUpdated?.Invoke(missionTime);
    }
}
