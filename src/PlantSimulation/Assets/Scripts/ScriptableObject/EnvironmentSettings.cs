using Assets.Scripts.Enum;
using UnityEngine;


[CreateAssetMenu]
public class EnvironmentSettings : ScriptableObject
{
    public LightColor Light = LightColor.Sun;
    public Water Water = Water.Optimal;
    public Temperature Temperature = Temperature.Optimal;
    public Minerals Minerals = Minerals.Optimal;
    public Oxygen Oxygen = Oxygen.Optimal;
    public CarbonDioxide CarbonDioxide = CarbonDioxide.Optimal;
}
