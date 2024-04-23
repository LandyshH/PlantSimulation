using System;
using UnityEngine;

[Serializable]
public struct FlowerComponent
{
    public float Size;
    public float Lifetime;
    public float maxSize;
    public Vector3 Position;
    public GameObject FlowerGO;
}
