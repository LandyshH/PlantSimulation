using Assets.Scripts.Enum;
using UnityEngine;


[CreateAssetMenu]
public class StaticData : ScriptableObject
{
    public GameObject SeedPrefab;
    public PlantGrowthStage PlantGrowthStage;
    public bool GoToNextStage;
    public float StemHeightDiff;
}

