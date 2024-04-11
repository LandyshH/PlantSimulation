using System;
using UnityEngine;

[Serializable]
public struct LeafComponent
{
    public float Size;
    public float Lifetime;
    public bool IsBud;
    public bool IsWaterLack;
    public Vector3 Position;
    public GameObject LeafGO;
    public float Height;
    public float Width;
}
