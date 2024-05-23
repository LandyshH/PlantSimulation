using Assets.Scripts.Enum;
using Leopotam.Ecs;


namespace Assets.Scripts.Systems.Lychnis
{
    public class FirstInitSystem : IEcsInitSystem
    {
        private EnvironmentSettings environment;
        private StaticData staticData;
        private readonly UI ui;

        public void Init()
        {
            staticData.PlantGrowthStage = PlantGrowthStage.Embryonic;
            ui.environmentWindowScreen.SetActive(false);
            staticData.JuvenileGenerated = false;
            staticData.SproutGenerated = false;
            staticData.MaturityGenerated = false;

            environment.Light = LightColor.Sun;
            environment.Water = Water.Optimal;
            environment.Temperature = Temperature.Optimal;
            environment.CarbonDioxide = CarbonDioxide.Optimal;
            environment.Oxygen = Oxygen.Optimal;
            environment.Minerals = Minerals.Optimal;
        }
    }
}
