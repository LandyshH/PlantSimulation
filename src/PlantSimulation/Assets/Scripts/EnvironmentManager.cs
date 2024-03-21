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
        Debug.Log("Change temp: " + environmentSettings.Temperature.ToString());
    }

    public void ChangeWaterValue()
    {
        environmentSettings.Water = (Water) WaterDropdown.value;
        Debug.Log("Change water: " + environmentSettings.Water.ToString());
    }

    public void ChangeMineralsValue()
    {
        environmentSettings.Minerals = (Minerals) MineralsDropdown.value;
        Debug.Log("Change minerals: " + environmentSettings.Minerals.ToString());
    }
    public void ChangeLightValue()
    {
        environmentSettings.Light = (LightColor) LightDropdown.value;
        Debug.Log("Change light: " + environmentSettings.Light.ToString());
    }

    public void ChangeOxygenValue()
    {
        environmentSettings.Oxygen = (Oxygen) OxygenDropdown.value;
        Debug.Log("Change oxygen: " + environmentSettings.Oxygen.ToString());
    }

    public void ChangeCarbonDioxideValue()
    {
        environmentSettings.CarbonDioxide = (CarbonDioxide) CarbonDioxideDropdown.value;
        Debug.Log("Change carbon dioxide: " + environmentSettings.CarbonDioxide.ToString());
    }
}
