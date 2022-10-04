using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Reward", menuName = "Scritable Objects/Reward")]
public class Reward : ScriptableObject
{

    public string ID;
    public Sprite RewardImage;

    public GameObject RewardPrefab;
    
   
}
