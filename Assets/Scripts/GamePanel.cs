using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanel : WG_Panel
{
    private void OnEnable()
    {
        EventManager.OnOpenGamePanel.AddListener(ShowPanel);
    }
    private void OnDisable()
    {
        EventManager.OnOpenGamePanel.RemoveListener(ShowPanel);
    }
}
