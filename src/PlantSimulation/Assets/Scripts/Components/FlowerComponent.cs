using System;
using UnityEngine;

[Serializable]
public struct FlowerComponent
{
    public float Size;
    public float Lifetime;
    public bool IsBud;
    public bool IsWaterLack;
    public Vector3 Position;
}