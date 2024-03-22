using Assets.Scripts.Enum;
using System;
using UnityEngine;

[Serializable]
public struct RootComponent
{
    public float Size;
    public float Lifetime;
    public Vector3 Position;
    public bool IsOxygenLack;
    //public PlantGrowthStage GrowthStage;
}
