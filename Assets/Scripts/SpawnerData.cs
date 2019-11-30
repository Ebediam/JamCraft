using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Spawner Data")]
public class SpawnerData : ScriptableObject
{
    public new string name;

    public string verb;
    public List<PickupData> spawnableItems;

    public PickupData toolRequired;

}
