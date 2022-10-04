using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : WG_Panel
{
    private Button startButton;
    protected Button StartButton { get { return (startButton == null) ? startButton = GetComponentInChildren<Button>() : startButton; } }

    private void OnValidate()
    {
        StartButton.onClick.AddListener(OpenGamePanel);
        
    }

    void OpenGamePanel()
    {
        HidePanel();
        EventManager.OnRestartGame.Invoke();
    }

}
