using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventControl : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            EventManager.OnLevelIncrease.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            EventManager.OnLevelDecrease.Invoke();
        }
    }
}
