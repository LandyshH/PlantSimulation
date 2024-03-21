using Assets.Scripts.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SeedComponent
{
    public SeedGrowthStage Stage;
    public float Size;
    public float Lifetime;
    public Vector3 Position;
    public GameObject gameObject;
}
