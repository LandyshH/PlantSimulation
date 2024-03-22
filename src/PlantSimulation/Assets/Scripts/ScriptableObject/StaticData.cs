using Assets.Scripts.Enum;
using UnityEngine;


[CreateAssetMenu]
public class StaticData : ScriptableObject
{
    public GameObject PlantPrefab;
    public bool SeedSpawned = false;
    public bool RootSpawned = false;
    public PlantGrowthStage PlantGrowthStage;
}

