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
            //уточнить логику роста
            foreach (var i in _stemFilter)
            {
                ref var stem = ref _stemFilter.Get1(i);

                stem.Lifetime += Time.deltaTime * 10;

                if (environment.Temperature == Temperature.Max || environment.Water == Water.Lack)
                {
                    if (environment.Temperature == Temperature.Max)
                    {
                        stem.Width -= 0.1f;
                    }

                    if (environment.Water == Water.Lack)
                    {
                        stem.Width -= 0.1f;
                    }
                }

                staticData.StemHeightDiff = GrowthRateCalculator.CalculateGrowthRate(environment) / 50;

                if (environment.CarbonDioxide != CarbonDioxide.Excess)
                {
                    staticData.StemHeightDiff = 0;
                }

                stem.Height += staticData.StemHeightDiff;
                stem.Width += GrowthRateCalculator.CalculateGrowthRate(environment) / 60;
            }
        }
    }
}
