using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    private Button _playButton;
    [SerializeField]
    private GameObject _gamePanel;
    private void OnValidate()
    {
        _playButton = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        _playButton.onClick.AddListener(OpenGamePanel);
    }
   
    void OpenGamePanel()
    {
        _gamePanel.SetActive(true);
        transform.parent.gameObject.SetActive(false);
    }
}
