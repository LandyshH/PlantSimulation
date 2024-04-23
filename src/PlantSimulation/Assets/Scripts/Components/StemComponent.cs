using System;
using UnityEngine;

[Serializable]
public struct StemComponent
{
    public float Height;
    public float MaxHeight;
    public float Width;
    public float MaxWidth;
    public float Lifetime;
    public float Water;
    public Vector3 Position;
    public GameObject stemGO;
    public int Generation;
    public bool IsGrowing;
}
