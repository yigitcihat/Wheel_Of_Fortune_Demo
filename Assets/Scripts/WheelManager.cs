using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;

public class WheelManager : MonoBehaviour
{
    private List<float> _partsAngles;
    private float _finalAngle;
    private float _startAngle = 0;
    private float _currentLerpRotTime;
    private float _anglePerItem;
    private bool _isStarted;
    private bool _isOneTime;
    private int _currentLevel;

    //------------------------
    public Button TurnButton;
    public GameObject Wheel;
    public GameObject Indicator;

    public List<Level> Levels = new List<Level>();

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

    private void GetReward()
    {
        float currentZPos = _startAngle;
        int awardWon = Mathf.Abs(((int)currentZPos + 360) / (int)_anglePerItem) % 360;
        Debug.Log("Pos z = " + currentZPos);
        Debug.Log("YOu get = " + Wheel.transform.GetChild(awardWon).GetChild(0).name);
        _isOneTime = false;
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

            int fullCircles = 5;
            float randomFinalAngle = _partsAngles[Random.Range(0, _partsAngles.Count)];

            _finalAngle = -(fullCircles * 360 + randomFinalAngle);
            _isStarted = true;
            _isOneTime = true;
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
                notify.Destroyed += Remove;
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
