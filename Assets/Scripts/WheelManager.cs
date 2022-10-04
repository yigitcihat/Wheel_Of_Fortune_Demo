using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class WheelManager : MonoBehaviour
{
    private static WheelManager _instance;

    public static WheelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("WheelManager");
                go.AddComponent<WheelManager>();
            }

            return _instance;
        }
    }

    private List<float> _partsAngles;
    private float _finalAngle;
    private float _startAngle = 0;
    private float _currentLerpRotTime;
    private float _anglePerItem;
    private bool _isStarted;
    //------------------------
    public int _currentLevel;
    public Button TurnButton;
    public GameObject Wheel;
    public RewardsHolder GainedRewardsHolder;
    public GameObject Indicator;
    public List<Level> Levels = new List<Level>();

    [SerializeField]
    private int _wheelPartsCount;
    void Awake()
    {
        _instance = this;

        //Sets the parts of angles in list
        SetAngles();
        //Places first rewards
        PlaceRewards();
    }
    private void OnEnable()
    {
        TurnButton.onClick.AddListener(TurnWheel);
        EventManager.OnRestartGame.AddListener(RestartGame);
    }
    private void OnDisable()
    {
        EventManager.OnRestartGame.RemoveListener(RestartGame);
    }


    void Update()
    {
        
        if (!_isStarted)
            return;

        float maxLerpRotationTime = 4f;

        _currentLerpRotTime += Time.deltaTime;
        if (_currentLerpRotTime > maxLerpRotationTime || Wheel.transform.eulerAngles.z == _finalAngle)
        {
            _currentLerpRotTime = maxLerpRotationTime;
            _isStarted = false;
            _startAngle = _finalAngle % 360;

            GetReward();

        }

        float t = _currentLerpRotTime / maxLerpRotationTime;

        // Speed up at start and speed down at the end of spining.
        t = t * t * t * (t * (6f * t - 15f) + 10f);

        float angle = Mathf.Lerp(_startAngle, _finalAngle, t);
        Wheel.transform.eulerAngles = new Vector3(0, 0, angle);

    }
    void RestartGame()
    {
        _currentLevel = 0;
        ClearOldRewards();
        PlaceRewards();

    }
    private void GetReward()
    {
        float currentZPos = _startAngle;
        int awardWon = (Mathf.Abs(((int)currentZPos + 360) / (int)_anglePerItem) % 360) % 8;
        Wheel.transform.GetChild(awardWon).GetComponentInChildren<TextMeshProUGUI>().transform.parent.GetComponent<Image>().enabled = false;
        Wheel.transform.GetChild(awardWon).SetParent(Wheel.transform);
        Wheel.transform.GetChild(awardWon).GetChild(0).DOLocalMove(Vector3.zero + new Vector3(0, -200, 0), 0.5f);
        Wheel.transform.GetChild(awardWon).GetChild(0).DOScale(new Vector3(2.5f, 2.5f, 2.5f), 0.5f);
        Wheel.transform.GetChild(awardWon).GetChild(0).GetComponentInChildren<TextMeshProUGUI>().transform.parent.GetComponent<RectTransform>().DOLocalMoveY(-40f, 0.5f);
        if (Wheel.transform.GetChild(awardWon).GetChild(0).GetComponent<WheelReward>().isBomb)
        {
            EventManager.OnOpenFailPanel.Invoke();
        }
        else
        {
            GainedRewardsHolder.AddRewardInHolder(Wheel.transform.GetChild(awardWon).GetChild(0).GetComponent<WheelReward>().rewardContent.gameObject);
            StartCoroutine(MoveRewardAnim(Wheel.transform.GetChild(awardWon).GetChild(0).GetChild(0).gameObject, GainedRewardsHolder._currentReward));
        }

    }

    private IEnumerator MoveRewardAnim(GameObject rewardObject, Transform parent)
    {
        rewardObject.SetActive(true);
        GameObject reward = rewardObject;
        for (int i = 0; i < 5; i++)
        {
            Vector3 startPos = rewardObject.GetComponent<RectTransform>().transform.position;
            GameObject a = Instantiate(rewardObject, rewardObject.GetComponent<RectTransform>().transform.position + new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100)), Quaternion.identity, rewardObject.transform.parent);
            
            a.transform.SetParent(parent);
            a.transform.GetComponent<RectTransform>().position = new Vector3(startPos.x + Random.Range(-200, 200), startPos.y / 3 , startPos.z);
            a.transform.GetComponent<RectTransform>().DOLocalMove(Vector3.zero, 1.5f).OnComplete(() => {
                Destroy(a.gameObject);
                });

        }

        yield return new WaitForSeconds(1.5f);
        parent.DOShakeScale(0.2f, 0.2f, 2, 45);

        parent.gameObject.GetComponent<RewardContent>().rewardCount += rewardObject.transform.parent.GetComponent<WheelReward>().Count;
        parent.gameObject.GetComponent<RewardContent>().UpdateRewardText();
        ClearOldRewards();
        PlaceRewards();
        EventManager.OnLevelIncrease.Invoke();
    }
    void SetAngles()
    {
        _partsAngles = new List<float>();
        _anglePerItem = 360 / _wheelPartsCount;
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

            int fullCircles = Random.Range(1, 10);
            float randomFinalAngle = _partsAngles[Random.Range(0, _partsAngles.Count)];

            _finalAngle = -(fullCircles * 360 + randomFinalAngle);
            _isStarted = true;
        }


    }

    void ClearOldRewards()
    {
        for (int i = 0; i < Wheel.transform.childCount; i++)
        {
            if (Wheel.transform.GetChild(i).childCount > 0)
            {
                Destroy(Wheel.transform.GetChild(i).GetChild(0).gameObject);
            }
        }
    }
    public void PlaceRewards()
    {
        if (_currentLevel < 0 || _currentLevel >= Levels.Count)
            return;
       
        for (int i = 0; i < _wheelPartsCount; i++)
        {
            GameObject a = Instantiate(Levels[_currentLevel].SlotReferences[i], Wheel.transform.GetChild(i));
            a.transform.localEulerAngles = Vector3.zero;
            if (Levels[_currentLevel].Slot_Rates[i] > 0)
            {
                a.GetComponentInChildren<WheelReward>().GetComponentInChildren<TextMeshProUGUI>().text = "X" + Levels[_currentLevel].Slot_Rates[i].ToString();
                a.GetComponentInChildren<WheelReward>().Count = Levels[_currentLevel].Slot_Rates[i];
            }

        }

        _currentLevel++;
    }


}




