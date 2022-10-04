using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class WheelManager : MonoBehaviour
{
    private List<float> _partsAngles;
    private float _finalAngle;
    private float _startAngle = 0;
    private float _currentLerpRotTime;
    private float _anglePerItem;
    private bool _isStarted;
    private int _currentLevel;

    //------------------------
    public Button TurnButton;
    public GameObject Wheel;
    public RewardsHolder GainedRewardsHolder;
    public GameObject Indicator;

    public List<Level> Levels = new List<Level>();

    //--------------------------------------------------
    [SerializeField]
    private int _wheelPartsCount;

    private void OnEnable()
    {
        EventManager.OnRestartGame.AddListener(RestartGame);
    }
    private void OnDisable()
    {
        EventManager.OnRestartGame.RemoveListener(RestartGame);
    }

    private void OnValidate()
    {
        TurnButton.onClick.AddListener(TurnWheel);
    }

    private void Awake()
    {
        
        //Sets the parts of angles in list
        SetAngles();
        //Places first rewards
        PlaceRewards();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlaceRewards();
            return;
        }
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
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    private void GetReward()
    {
        float currentZPos = _startAngle;
        int awardWon = (Mathf.Abs(((int)currentZPos + 360) / (int)_anglePerItem) % 360) % 8;
        Debug.Log("Award sort = " + awardWon);
        Debug.Log("YOu get = " + Wheel.transform.GetChild(awardWon).GetChild(0).name);
        Wheel.transform.GetChild(awardWon).GetComponentInChildren<TextMeshProUGUI>().transform.parent.GetComponent<Image>().enabled = false;
        Wheel.transform.GetChild(awardWon).SetParent(Wheel.transform);
        Wheel.transform.GetChild(awardWon).GetChild(0).DOLocalMove(Vector3.zero + new Vector3(0,-200,0), 0.5f);
        Wheel.transform.GetChild(awardWon).GetChild(0).DOScale(new Vector3(2.5f, 2.5f, 2.5f), 0.5f);
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
        GameObject reward = rewardObject;
        for (int i = 0; i < 5; i++)
        {
            Vector3 startPos = rewardObject.GetComponent<RectTransform>().transform.position;
            GameObject a =  Instantiate(rewardObject, rewardObject.GetComponent<RectTransform>().transform.position+ new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100)), Quaternion.identity, rewardObject.transform.parent);
           
            a.transform.SetParent(parent);
            a.transform.GetComponent<RectTransform>().position = new Vector3(startPos.x,startPos.y/3,startPos.z);
            //a.transform.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(Vector3.zero);
            a.transform.GetComponent<RectTransform>().DOLocalMove(Vector3.zero, 2f).OnComplete(()=>Destroy(a.gameObject));
            
        }
       
        yield return new WaitForSeconds(2f);
        parent.DOShakeScale(0.2f, 0.2f, 2, 45);

        parent.gameObject.GetComponent<RewardContent>().rewardCount += System.Convert.ToInt32(rewardObject.transform.parent.GetComponentInChildren<TextMeshProUGUI>().text.Split("X")[1]);
        parent.gameObject.GetComponent<RewardContent>().UpdateRewardText();

        PlaceRewards(); 
        EventManager.OnLevelIncrease.Invoke();
    }
    public Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos)
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
        //Convert the local point to world point
        return parentCanvas.transform.TransformPoint(movePos);
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

            int fullCircles = Random.Range(1,10);
            float randomFinalAngle = _partsAngles[Random.Range(0, _partsAngles.Count)];

            _finalAngle = -(fullCircles * 360 + randomFinalAngle);
            _isStarted = true;
        }


    }

    void ClearOldRewards()
    {
        for (int i = 0; i < Wheel.transform.childCount; i++)
        {
            if (Wheel.transform.GetChild(i).childCount >0)
            {
                Destroy(Wheel.transform.GetChild(i).GetChild(0).gameObject);
            }
        }
    }
    public void PlaceRewards()
    {
        if (_currentLevel < 0 || _currentLevel >= Levels.Count)
            return;
        ClearOldRewards();
        
        for (int i = 0; i < _wheelPartsCount; i++)
        {
            Debug.Log(_currentLevel);
            AssetReference assetReference = Levels[_currentLevel].SlotReferences[i];
           
            if (assetReference.RuntimeKeyIsValid() == false)
            {
                Debug.Log("Invalid Key" + assetReference.RuntimeKey.ToString());
                return;
            }
            if (Levels[_currentLevel].AsyncOperationHandles.ContainsKey(assetReference))
            {
                if (Levels[_currentLevel].AsyncOperationHandles[assetReference].IsDone)
                    SpawnRewardInSlotFromReference(assetReference, Wheel.transform.GetChild(i).transform.position, i);
                else
                    EnqueueSpawnForAfterInitialization(assetReference, i);
                return;
            }

            LoadAndSpawn(assetReference, i);
            Debug.Log("rEWARDS REFRESHED");
        }

        _currentLevel++;
    }

    //-----------------------------Adressable Methods --------------------------
    private void LoadAndSpawn(AssetReference assetReference, int slotNum)
    {
        var op = Addressables.LoadAssetAsync<GameObject>(assetReference);
        Levels[_currentLevel].AsyncOperationHandles[assetReference] = op;
        op.Completed += (operation) =>
            {
                SpawnRewardInSlotFromReference(assetReference, Wheel.transform.GetChild(slotNum).transform.position, slotNum);
                if (Levels[_currentLevel].QueuedCreatedRequests.ContainsKey(assetReference))
                {
                    while (Levels[_currentLevel].QueuedCreatedRequests[assetReference]?.Any() == true)
                    {
                        var position = Levels[_currentLevel].QueuedCreatedRequests[assetReference].Dequeue();
                        SpawnRewardInSlotFromReference(assetReference, position, slotNum);
                    }
                }
            };
    }

    private void EnqueueSpawnForAfterInitialization(AssetReference assetReference, int slotNum)
    {
        if (Levels[_currentLevel].QueuedCreatedRequests.ContainsKey(assetReference) == false)
            Levels[_currentLevel].QueuedCreatedRequests[assetReference] = new Queue<Vector3>();
        Levels[_currentLevel].QueuedCreatedRequests[assetReference].Enqueue(Wheel.transform.GetChild(slotNum).transform.position);


    }

    private void SpawnRewardInSlotFromReference(AssetReference assetReference, Vector3 position, int slotNum)
    {
        assetReference.InstantiateAsync(position, Quaternion.identity, Wheel.transform.GetChild(slotNum).transform).Completed += (asyncOperationHandle) =>
            {
                if (Levels[_currentLevel].CreatedSlotSystems.ContainsKey(assetReference) == false)
                {
                    Levels[_currentLevel].CreatedSlotSystems[assetReference] = new List<GameObject>();
                }
                Levels[_currentLevel].CreatedSlotSystems[assetReference].Add(asyncOperationHandle.Result);
                var notify = asyncOperationHandle.Result.AddComponent<NotifyOnDestroy>();
                asyncOperationHandle.Result.transform.localEulerAngles = Vector3.zero;
                if (_currentLevel == 0)
                {
                    if (Levels[0].Slot_Rates[slotNum] > 0)
                    {
                        asyncOperationHandle.Result.GetComponentInChildren<TextMeshProUGUI>().text = "X" + Levels[0].Slot_Rates[slotNum].ToString();
                    }
                }
                else
                {
                    if (Levels[_currentLevel - 1].Slot_Rates[slotNum] > 0)
                    {
                        asyncOperationHandle.Result.GetComponentInChildren<TextMeshProUGUI>().text = "X" + Levels[_currentLevel - 1].Slot_Rates[slotNum].ToString();
                    }
                }
                
               
              

              
                
                //notify.Destroyed += Remove;
                notify.AssetReference = assetReference;


            };
    }
    
    private void Remove(AssetReference assetReference, NotifyOnDestroy obj)
    {
        Addressables.ReleaseInstance(obj.gameObject);

        Levels[_currentLevel].CreatedSlotSystems[assetReference].Remove(obj.gameObject);
        if (Levels[_currentLevel].CreatedSlotSystems[assetReference].Count == 0)
        {
            Debug.Log($"Removed all {assetReference.RuntimeKey.ToString()}");

            if (Levels[_currentLevel].AsyncOperationHandles[assetReference].IsValid())
                Addressables.Release(Levels[_currentLevel].AsyncOperationHandles[assetReference]);

            Levels[_currentLevel].AsyncOperationHandles.Remove(assetReference);
        }
    }
    //-------------------------------------------------------
    


   
}
