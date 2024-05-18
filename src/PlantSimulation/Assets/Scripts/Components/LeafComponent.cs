using System;
using UnityEngine;

[Serializable]
public struct LeafComponent
{
    public float Size;
    public float maxSize;
    public float Lifetime;
    public bool IsWaterLack;
    public Vector3 Position;
    public GameObject LeafGO;
    public float Height;
    public float Width;
    public float MaxHeight;
    public float MaxWidth;
    public int LeafNumber;
    public bool ChangedToRust;

    public Vector3 TargetPosition;
}
