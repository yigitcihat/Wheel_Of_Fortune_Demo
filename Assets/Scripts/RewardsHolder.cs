using System.Collections.Generic;
using UnityEngine;

public class RewardsHolder : MonoBehaviour
{

    public List<GameObject> rewardsPrefs = new List<GameObject>();
    List<Reward> _gainedRewards = new List<Reward>();
    public List<GameObject> GainedRewardsObjects = new List<GameObject>();
    public Transform _currentReward;
    private void OnEnable()
    {
        EventManager.OnRestartGame.AddListener(ResetRewerds);
    }
    private void OnDisable()
    {
        EventManager.OnRestartGame.RemoveListener(ResetRewerds);
    }
    private void ResetRewerds()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Debug.Log(transform.GetChild(i).gameObject.name);
            Destroy(transform.GetChild(i).gameObject);
        }
        rewardsPrefs.Clear();
        _gainedRewards.Clear();
        GainedRewardsObjects.Clear();
    }
    public void AddRewardInHolder(GameObject rewardObj)
    {
        if (!_gainedRewards.Contains(rewardObj.GetComponent<RewardContent>().reward))
        {
            //Debug.Log("Created");
            _gainedRewards.Add(rewardObj.GetComponent<RewardContent>().reward);
            _currentReward = Instantiate(rewardObj, transform).transform;
            GainedRewardsObjects.Add(_currentReward.gameObject);
        }
        else
        {
            _currentReward = GainedRewardsObjects[_gainedRewards.IndexOf(rewardObj.GetComponent<RewardContent>().reward)].transform;
        }
    }




}
