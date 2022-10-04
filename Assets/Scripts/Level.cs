using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;
[CreateAssetMenu (fileName = "New Level", menuName = "Scritable Objects/Level")]
public class Level : ScriptableObject 
{
    public int levelIndex;

    public  List<GameObject> SlotReferences = new List<GameObject>();
   

    public int[] Slot_Rates = new int[8];

    

}
