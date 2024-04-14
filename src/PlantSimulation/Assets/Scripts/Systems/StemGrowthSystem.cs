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
            if (staticData.PlantGrowthStage == PlantGrowthStage.Senile)
            {
                return;
            }

            if (staticData.GoToNextStage)
            {
                return;
            }

            
            foreach (var i in _stemFilter)
            {
                ref var stem = ref _stemFilter.Get1(i);

                stem.Lifetime += Time.deltaTime * 10;

                if (environment.Temperature == Temperature.Max || environment.Water == Water.Lack)
                {
                    if (stem.Width > 1.5f)
                    {
                        if (environment.Temperature == Temperature.Max)
                        {
                            stem.Width -= 0.01f;
                        }

                        if (environment.Water == Water.Lack)
                        {
                            stem.Width -= 0.01f;
                        }
                    }
                }
                else if (environment.Light == LightColor.Blue)
                {
                    stem.Width += 0.01f;
                }
                

                staticData.StemHeightDiff += GrowthRateCalculator.CalculateGrowthRate(environment) * 0.00001f;

                if (environment.CarbonDioxide == CarbonDioxide.Excess)
                {
                    staticData.StemHeightDiff = 0;
                }

                stem.Height += staticData.StemHeightDiff;
                stem.Width += GrowthRateCalculator.CalculateGrowthRate(environment) * 0.004f;
            }
        }
    }
}
