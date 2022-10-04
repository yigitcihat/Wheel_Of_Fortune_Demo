using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardContent : MonoBehaviour
{
    public Reward reward;
    public int rewardCount;



    public void UpdateRewardText()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = "X" + rewardCount;
    }
    private void OnValidate()
    {
        //transform.GetComponentInChildren<Image>().sprite = reward.RewardImage;
        //transform.GetComponentInChildren<TextMeshProUGUI>().text = "";
    }
  
}
