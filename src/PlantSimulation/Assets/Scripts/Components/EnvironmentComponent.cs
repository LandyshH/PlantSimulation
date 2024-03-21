using System;
using Assets.Scripts.Enum;

[Serializable]
public struct EnvironmentComponent
{
    public LightColor Light;
    public Water Water;
    public Temperature Temperature;
    public Minerals Minerals;
    public Oxygen Oxygen;
    public CarbonDioxide CarbonDioxide;

    // избыток воды -- больше углекислого газа
}
