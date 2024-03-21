using System;
using UnityEngine;

[Serializable]
public struct BranchComponent
{
    public float Height;
    public float Width;
    public float Lifetime;
    public bool HasLeafOrFlower;
    public Vector3 Position;
}
