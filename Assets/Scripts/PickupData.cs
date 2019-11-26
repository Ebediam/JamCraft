using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Pickup", menuName = "Pickup")]
public class PickupData : ScriptableObject
{
    public new string name;
    public Sprite icon;

    public string verb;

    public string description;

    public PickupData requiredTool;

    public GameObject prefab;
    public bool showsInPlayer;

    public int amount;

    public bool remainsAfterCombination;


}
