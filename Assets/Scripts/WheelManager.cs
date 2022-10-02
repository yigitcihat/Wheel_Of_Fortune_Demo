using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelManager : MonoBehaviour
{
    private List<float> _partsAngles;
    private float _finalAngle;
    private float _startAngle = 0;
    private float _currentLerpRotTime;
    private bool _isStarted;

    //------------------------
    public Button TurnButton;
    public GameObject Circle;

    //--------------------------------------------------
    [SerializeField]
    private int _wheelPartsCount;

    private void OnValidate()
    {
        TurnButton.onClick.AddListener(TurnWheel);
    }

    private void Awake()
    {
        //Sets the parts of angles in list
        SetAngles();
    }

    void SetAngles()
    {
        _partsAngles = new List<float>();
        float _anglePerItem = 360 / _wheelPartsCount;
        float _currentAngle = 0;
        for (int i = 0; i < _wheelPartsCount; i++)
        {
            _currentAngle += _anglePerItem;
            _partsAngles.Add(_currentAngle);
        }
    }


    public void TurnWheel()
    {
        if (!_isStarted)
        {
            _currentLerpRotTime = 0f;

            int fullCircles = 5;
            float randomFinalAngle = _partsAngles[Random.Range(0, _partsAngles.Count)];

            _finalAngle = -(fullCircles * 360 + randomFinalAngle);
            _isStarted = true;
        }


    }

    //private void GiveAwardByAngle()
    //{
    //    // Here you can set up rewards for every sector of wheel
    //    switch ((int)_startAngle)
    //    {
    //        case 0:
    //            RewardCoins(1000);
    //            break;
    //        case -330:
    //            RewardCoins(200);
    //            break;
    //        case -300:
    //            RewardCoins(100);
    //            break;
    //        case -270:
    //            RewardCoins(500);
    //            break;
    //        case -240:
    //            RewardCoins(300);
    //            break;
    //        case -210:
    //            RewardCoins(100);
    //            break;
    //        case -180:
    //            RewardCoins(900);
    //            break;
    //        case -150:
    //            RewardCoins(200);
    //            break;
    //        case -120:
    //            RewardCoins(100);
    //            break;
    //        case -90:
    //            RewardCoins(700);
    //            break;
    //        case -60:
    //            RewardCoins(300);
    //            break;
    //        case -30:
    //            RewardCoins(100);
    //            break;
    //        default:
    //            RewardCoins(300);
    //            break;
    //    }
    //}

    void Update()
    {
        // Make turn button non interactable if user has not enough money for the turn
        //if (_isStarted || CurrentCoinsAmount < TurnCost)
        //{
        //TurnButton.interactable = false;
        //TurnButton.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
        //}
        //else
        ////{
        //    TurnButton.interactable = true;
        //    TurnButton.GetComponent<Image>().color = new Color(255, 255, 255, 1);
        //}

        if (!_isStarted)
            return;

        float maxLerpRotationTime = 4f;

        // increment timer once per frame
        _currentLerpRotTime += Time.deltaTime;
        if (_currentLerpRotTime > maxLerpRotationTime || Circle.transform.eulerAngles.z == _finalAngle)
        {
            _currentLerpRotTime = maxLerpRotationTime;
            _isStarted = false;
            _startAngle = _finalAngle % 360;

            //GiveAwardByAngle();
            //StartCoroutine(HideCoinsDelta());
        }

        // Calculate current position using linear interpolation
        float t = _currentLerpRotTime / maxLerpRotationTime;

        // This formulae allows to speed up at start and speed down at the end of rotation.
        // Try to change this values to customize the speed
        t = t * t * t * (t * (6f * t - 15f) + 10f);

        float angle = Mathf.Lerp(_startAngle, _finalAngle, t);
        Circle.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    //private void RewardCoins(int awardCoins)
    //{
    //    CurrentCoinsAmount += awardCoins;
    //    CoinsDeltaText.text = "+" + awardCoins;
    //    CoinsDeltaText.gameObject.SetActive(true);
    //    StartCoroutine(UpdateCoinsAmount());
    //}

    //private IEnumerator HideCoinsDelta()
    //{
    //    yield return new WaitForSeconds(1f);
    //    CoinsDeltaText.gameObject.SetActive(false);
    //}

    //private IEnumerator UpdateCoinsAmount()
    //{
    //    // Animation for increasing and decreasing of coins amount
    //    const float seconds = 0.5f;
    //    float elapsedTime = 0;

    //    while (elapsedTime < seconds)
    //    {
    //        CurrentCoinsText.text = Mathf.Floor(Mathf.Lerp(PreviousCoinsAmount, CurrentCoinsAmount, (elapsedTime / seconds))).ToString();
    //        elapsedTime += Time.deltaTime;

    //        yield return new WaitForEndOfFrame();
    //    }

    //    PreviousCoinsAmount = CurrentCoinsAmount;
    //    CurrentCoinsText.text = CurrentCoinsAmount.ToString();
    //}
}
