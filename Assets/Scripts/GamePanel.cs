using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : WG_Panel
{
    private Button closeButton;
    protected Button CloseButton { get { return (closeButton == null) ? closeButton = transform.GetChild(0).GetComponentInChildren<Button>() : closeButton; } }
    private void OnEnable()
    {
        EventManager.OnOpenGamePanel.AddListener(ShowPanel);
    }
    private void OnDisable()
    {
        EventManager.OnOpenGamePanel.RemoveListener(ShowPanel);
    }
    private void Start()
    {
        CloseButton.transform.parent = CloseButton.transform.parent.parent;
        CloseButton.transform.SetAsLastSibling();
    }

    
    private void OnValidate()
    {
        CloseButton.onClick.AddListener(CloseGamePanel);
    }

    void CloseGamePanel()
    {
        HidePanel();
        EventManager.OnCloseGamePanel.Invoke();


    }
}
