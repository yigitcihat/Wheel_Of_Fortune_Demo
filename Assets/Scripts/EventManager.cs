using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;


public static class EventManager
{
    public static UnityEvent OnLevelIncrease = new UnityEvent();
    public static UnityEvent OnOpenGamePanel = new UnityEvent();
    public static UnityEvent OnCloseGamePanel = new UnityEvent();
    public static UnityEvent OnOpenFailPanel = new UnityEvent();
    public static UnityEvent OnRestartGame = new UnityEvent();
}

