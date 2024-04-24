using Assets.Scripts.Enum;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class StaticData : ScriptableObject
{
    public GameObject SeedPrefab;
    public PlantGrowthStage PlantGrowthStage;
    public bool GoToNextStage;
    public float StemHeightDiff;
    public int PetalCount = 15;
    public List<Vector3> leafPositions;
    public int Generations = 17;
    public int CurrGeneration = 0;

    public bool SproutGenerated = false;
    public bool JuvnileGenerated = false;
    public bool MaturityGenerated = false;
    public bool SenileGenerated = false;
}

