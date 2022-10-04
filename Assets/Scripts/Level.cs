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

    public  List<AssetReference> SlotReferences;
    [HideInInspector]
    public readonly Dictionary<AssetReference,List<GameObject>> CreatedSlotSystems =
        new Dictionary<AssetReference,List<GameObject>>();
    [HideInInspector]
    public readonly Dictionary<AssetReference,Queue<Vector3>> QueuedCreatedRequests =
          new Dictionary<AssetReference, Queue<Vector3>>();
    [HideInInspector]
    public readonly Dictionary<AssetReference, AsyncOperationHandle<GameObject>> AsyncOperationHandles =
         new Dictionary<AssetReference, AsyncOperationHandle<GameObject>>();

    public int[] Slot_Rates = new int[8];

    

}
