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
            if (staticData.PlantGrowthStage == PlantGrowthStage.Senile || staticData.GoToNextStage)
            {
                return;
            }

            foreach (var i in _stemFilter)
            {
                ref var stem = ref _stemFilter.Get1(i);

                stem.Lifetime += Time.deltaTime;

                if (environment.Temperature == Temperature.Max || environment.Water == Water.Lack)
                {
                    stem.MaxHeight = 12f;

                    if (stem.Width >= 1.3f)
                    {
                        if (environment.Temperature == Temperature.Max)
                        {
                            stem.Width -= 0.004f;
                        }

                        if (environment.Water == Water.Lack)
                        {
                            stem.Width -= 0.002f;
                        }
                    }
                }
                else if (environment.Light == LightColor.Blue)
                {
                    stem.Width += 0.01f;
                }
                else if (environment.Light == LightColor.Red)
                {
                    stem.MaxHeight = 18f;
                }
                else
                {
                    if (stem.Width < stem.MaxWidth)
                    {
                        stem.Width += GrowthRateCalculator.CalculateGrowthRate(environment) * Time.deltaTime * 0.3f;
                    }
                }

                if (stem.Height < stem.MaxHeight)
                {
                    stem.Height += GrowthRateCalculator.CalculateGrowthRate(environment) * Time.deltaTime * 0.5f;
                }
            }
        }
    }
}
