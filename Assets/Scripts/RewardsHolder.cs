using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardsHolder : MonoBehaviour
{

    public List<GameObject> rewardsPrefs = new List<GameObject>();
    List<Reward> _gainedRewards = new List<Reward>();
    public List<GameObject> GainedRewardsObjects = new List<GameObject>();
    public Transform _currentReward;
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
