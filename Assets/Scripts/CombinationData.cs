using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Combination", menuName = "Define Combination")]
public class CombinationData : ScriptableObject
{
    public PickupData firstItem;
    public PickupData secondItem;

    public PickupData craftedItem;
}
