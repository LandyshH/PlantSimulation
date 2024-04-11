using Assets.Scripts.Enum;
using System;
using UnityEngine;

[Serializable]
public struct StemComponent
{
    public float Height;
    public float Width;
    public float Lifetime;
    //public PlantGrowthStage GrowthStage;
    public float Water;
    public Vector3 Position;
    public GameObject stemGO;
}

// Width < x => ������
// Water < y => ������