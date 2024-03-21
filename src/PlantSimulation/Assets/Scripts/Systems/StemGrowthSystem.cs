using Assets.Scripts.Enum;
using Assets.Scripts.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public sealed class StemGrowthSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private EnvironmentSettings environment;
        private StaticData staticData;

        EcsFilter<StemComponent> _stemFilter;
        EcsFilter<RootComponent> _rootFilter;

        public void Run()
        {
            if (!staticData.SeedSpawned)
            {
                return;
            }

            ref var root = ref _rootFilter.Get1(0);

            foreach (var i in _stemFilter)
            {
                ref var stem = ref _stemFilter.Get1(i);

                stem.GrowthStage = root.GrowthStage;
                Debug.Log("Stem grow " + stem.GrowthStage + stem.Height + " " + stem.Width + " " + stem.Lifetime);


                if (stem.GrowthStage == PlantGrowthStage.Senile || stem.GrowthStage == PlantGrowthStage.Embryonic)
                {
                    continue;
                }

                //ref var stemEntity = ref _filter.GetEntity(i);

                //ref var environment = ref _filter.Get2(i);

                stem.Lifetime += Time.deltaTime * 10;

                if (environment.CarbonDioxide != CarbonDioxide.Excess)
                {
                    if (environment.Temperature == Temperature.Max || environment.Water == Water.Lack)
                    {
                        stem.Height += GrowthRateCalculator.CalculateGrowthRate(environment) / 10;

                        if (environment.Temperature == Temperature.Max)
                        {
                            stem.Width -= 1;
                        }

                        if (environment.Water == Water.Lack)
                        {
                            stem.Width -= 1;
                        }
                        
                        continue;
                    }

                    if (environment.Temperature == Temperature.Min
                        || environment.Oxygen == Oxygen.Lack || environment.Oxygen == Oxygen.Excess
                        || environment.Light == LightColor.Darkness)
                    {
                        stem.Height += GrowthRateCalculator.CalculateGrowthRate(environment) / 10;
                        stem.Width += GrowthRateCalculator.CalculateGrowthRate(environment) / 5;
                        continue;
                    }

                    if (environment.Light == LightColor.Blue)
                    {
                        stem.Width += GrowthRateCalculator.CalculateGrowthRate(environment) / 5;
                    }

                    if (environment.Light == LightColor.Red)
                    {
                        stem.Height += GrowthRateCalculator.CalculateGrowthRate(environment) / 10;
                    }

                    stem.Height += GrowthRateCalculator.CalculateGrowthRate(environment) / 10;
                    stem.Width += GrowthRateCalculator.CalculateGrowthRate(environment) / 5;

                    if (stem.Width <= 1)
                    {
                        //usohlo
                    }
                }

            }
        }
    }
}
