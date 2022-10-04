using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailPanel : WG_Panel
{

    private Button startButton;
    protected Button StartButton { get { return (startButton == null) ? startButton = GetComponentInChildren<Button>() : startButton; } }

    private void OnEnable()
    {
        EventManager.OnOpenFailPanel.AddListener(ShowPanel);
    }
    private void OnDisable()
    {
        EventManager.OnOpenFailPanel.RemoveListener(ShowPanel);
    }
    private void OnValidate()
    {
        StartButton.onClick.AddListener(RestartGame);

    }

    void RestartGame()
    {
        EventManager.OnRestartGame.Invoke();
        HidePanel();
        
    }
}
