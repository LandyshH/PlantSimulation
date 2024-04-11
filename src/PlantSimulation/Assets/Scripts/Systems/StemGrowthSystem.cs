using Assets.Scripts.Enum;
using Assets.Scripts.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public sealed class StemGrowthSystem : IEcsRunSystem
    {
        private EnvironmentSettings environment;
        private StaticData staticData;
        //private PlantData plantData;

        EcsFilter<StemComponent> _stemFilter;

        public void Run()
        {
            if (staticData.GoToNextStage)
            {
                return;
            }

            if (staticData.PlantGrowthStage == PlantGrowthStage.Senile || staticData.PlantGrowthStage == PlantGrowthStage.Embryonic)
            {
                return;
            }

            foreach (var i in _stemFilter)
            {
                ref var stem = ref _stemFilter.Get1(i);

                stem.Lifetime += Time.deltaTime * 10;

                if (environment.CarbonDioxide != CarbonDioxide.Excess)
                {
                    if (environment.Temperature == Temperature.Max || environment.Water == Water.Lack)
                    {
                        stem.Height += GrowthRateCalculator.CalculateGrowthRate(environment) / 50;

                        if (environment.Temperature == Temperature.Max)
                        {
                            stem.Width -= 0.1f;
                        }

                        if (environment.Water == Water.Lack)
                        {
                            stem.Width -= 0.1f;
                        }

                        return;
                    }

                    if (environment.Temperature == Temperature.Min
                        || environment.Oxygen == Oxygen.Lack || environment.Oxygen == Oxygen.Excess
                        || environment.Light == LightColor.Darkness)
                    {
                        stem.Height += GrowthRateCalculator.CalculateGrowthRate(environment) / 50;
                        stem.Width += GrowthRateCalculator.CalculateGrowthRate(environment) / 60;

                        return;
                    }

                    if (environment.Light == LightColor.Blue)
                    {
                        stem.Width += GrowthRateCalculator.CalculateGrowthRate(environment) / 60;
                    }

                    if (environment.Light == LightColor.Red)
                    {
                        stem.Height += GrowthRateCalculator.CalculateGrowthRate(environment) / 50;
                    }

                    stem.Height += GrowthRateCalculator.CalculateGrowthRate(environment) / 50;
                    stem.Width += GrowthRateCalculator.CalculateGrowthRate(environment) / 60;
                }

                /*plantData.stemWidth += stem.Width;
                plantData.stemHeight += stem.Height;*/
            }
        }
    }
}
