using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : WG_Panel
{
    private Button startButton;
    protected Button StartButton { get { return (startButton == null) ? startButton = GetComponentInChildren<Button>() : startButton; } }

    private void OnEnable()
    {
        StartButton.onClick.AddListener(OpenGamePanel);
        EventManager.OnCloseGamePanel.AddListener(ShowPanel);
    }
    private void OnDisable()
    {
        EventManager.OnCloseGamePanel.RemoveListener(ShowPanel);
    }


    void OpenGamePanel()
    {
        HidePanel();
        EventManager.OnOpenGamePanel.Invoke();
    }

}
