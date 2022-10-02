using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (fileName = "New Level", menuName = "Scritable Objects/Level")]
public class Level : ScriptableObject 
{

    public int levelIndex;

    public Image[] Slot_1_Images = new Image[8];

    public string[] Slot_1_Names = new string[8];

    public int[] Slot_1_Rates = new int[8];

    public Color32[] Slot_1_Colors = new Color32[8];


}
