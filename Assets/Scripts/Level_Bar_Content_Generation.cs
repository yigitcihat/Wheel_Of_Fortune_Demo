using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Level_Bar_Content_Generation : MonoBehaviour
{
    [SerializeField]
    private int _level_Count = 30;

    [SerializeField]
    private GameObject _level_content;



    private void Start()
    {
        _level_content = transform.GetChild(0).gameObject;
        SetContentInfo(_level_content, 1);
        for (int i = 2; i <= _level_Count; i++)
        {
            GameObject current_level_content = Instantiate(_level_content, transform);
            SetContentInfo(current_level_content, i);
        }
    }
   

    void SetContentInfo(GameObject content, int level)
    {
        content.gameObject.name += _level_content.name + "_" + level;
        content.GetComponentInChildren<TextMeshProUGUI>().text = level.ToString();
        if (level == 1 || level%5 == 0 )
        {
            content.GetComponentInChildren<Image>().color = new Color32(114, 255, 83, 255);
        }
        else
        {
            content.GetComponentInChildren<Image>().color = new Color32(47,130,255,255);
        }
        
    }
}
