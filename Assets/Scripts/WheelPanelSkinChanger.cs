using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelPanelSkinChanger : MonoBehaviour
{

    public Sprite BronzeSpin;
    public Sprite BronzeIndicator;
    public Sprite SilverSpin;
    public Sprite SilverIndicator;
    public Sprite GoldSpin;
    public Sprite GoldIndicator;

    private Image _wheelImage;
    private Image _indicatorlImage;


    private void Start()
    {
        _wheelImage = transform.GetChild(0).GetComponent<Image>();
        _indicatorlImage = transform.GetChild(2).GetComponent<Image>();
    }

    private void OnEnable()
    {
        EventManager.OnLevelIncrease.AddListener(ChangeWheelSkin);
        EventManager.OnRestartGame.AddListener(ChangeWheelSkin);
    }
    private void OnDisable()
    {
        EventManager.OnLevelIncrease.RemoveListener(ChangeWheelSkin);
        EventManager.OnRestartGame.RemoveListener(ChangeWheelSkin);
    }

    void ChangeWheelSkin()
    {
        if (WheelManager.Instance._currentLevel == 30)
        {
            _wheelImage.sprite = GoldSpin;
            _indicatorlImage.sprite = GoldIndicator;
        }
        else if ((WheelManager.Instance._currentLevel ) %5 == 0)
        {
            _wheelImage.sprite = SilverSpin;
            _indicatorlImage.sprite = SilverIndicator;
        }
        else
        {
            _wheelImage.sprite = BronzeSpin;
            _indicatorlImage.sprite = BronzeIndicator;
        }
       
    }

}
