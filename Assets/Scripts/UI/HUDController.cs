using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI reverseToggleText;
    [SerializeField] TextMeshProUGUI missionTimeText;

    private void OnEnable()
    {
        PowerboatMovement.OnSpeedChanged += SetSpeedText;
        PowerboatMovement.OnReverseToggled += SetReverseText;
    }

    private void OnDisable()
    {
        PowerboatMovement.OnSpeedChanged -= SetSpeedText;
        PowerboatMovement.OnReverseToggled -= SetReverseText;
    }

    private void SetSpeedText(float speed)
    {
        speedText.text = speed.ToString();
    }

    private void SetReverseText(bool isReversing)
    {
        if (isReversing)
        {
            reverseToggleText.color = Color.green;
        }
        else
        {
            reverseToggleText.color = Color.white;
        }
    }


    public void ResetGame()
    {
        // Loads the current scene (resets game)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
