using Assets.Scripts;
using Assets.Scripts.Enum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnvironmentManager : MonoBehaviour
{
    public EnvironmentSettings environmentSettings;

    [SerializeField] private TMP_Dropdown TemperatureDropdown;
    [SerializeField] private TMP_Dropdown WaterDropdown;
    [SerializeField] private TMP_Dropdown MineralsDropdown;
    [SerializeField] private TMP_Dropdown LightDropdown;
    [SerializeField] private TMP_Dropdown OxygenDropdown;
    [SerializeField] private TMP_Dropdown CarbonDioxideDropdown;

    public void ChangeTemperatureValue()
    {
        environmentSettings.Temperature = (Temperature) TemperatureDropdown.value;
    }

    public void ChangeWaterValue()
    {
        environmentSettings.Water = (Water) WaterDropdown.value;
    }

    public void ChangeMineralsValue()
    {
        environmentSettings.Minerals = (Minerals) MineralsDropdown.value;
    }
    public void ChangeLightValue()
    {
        environmentSettings.Light = (LightColor) LightDropdown.value;
    }

    public void ChangeOxygenValue()
    {
        environmentSettings.Oxygen = (Oxygen) OxygenDropdown.value;
    }

    public void ChangeCarbonDioxideValue()
    {
        environmentSettings.CarbonDioxide = (CarbonDioxide) CarbonDioxideDropdown.value;
    }
}
