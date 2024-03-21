using Assets.Scripts.Enum;
using UnityEngine;


[CreateAssetMenu]
public class EnvironmentSettings : ScriptableObject
{
    public LightColor Light;
    public Water Water;
    public Temperature Temperature;
    public Minerals Minerals;
    public Oxygen Oxygen;
    public CarbonDioxide CarbonDioxide;
}
