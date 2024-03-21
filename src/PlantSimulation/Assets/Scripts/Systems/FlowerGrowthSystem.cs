using Assets.Scripts.Services;
using Leopotam.Ecs;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public sealed class FlowerGrowthSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private EnvironmentSettings environment;

        EcsFilter<FlowerComponent> _flowerFilter;
        EcsFilter<StemComponent> _stemFilter;

        public void Run()
        {
            ref var stem = ref _stemFilter.Get1(0);

            foreach (var i in _flowerFilter)
            {
                if (stem.GrowthStage != Enum.PlantGrowthStage.MaturityAndReproduction)
                {
                    continue;
                }

                ref var flower = ref _flowerFilter.Get1(i);

                Debug.Log("Flower grow " + flower.Size + " " + flower.Lifetime);
                ref var entity = ref _flowerFilter.GetEntity(i);

                //ref var environment = ref _filter.Get2(i);

                if (environment.Temperature != Enum.Temperature.Optimal)
                {
                    flower.Size += GrowthRateCalculator.CalculateGrowthRate(environment) * Time.deltaTime / 10;
                    flower.Lifetime += 10 * Time.deltaTime;
                    continue;
                }

                flower.Size += GrowthRateCalculator.CalculateGrowthRate(environment) * Time.deltaTime;
                flower.Lifetime += 10 * Time.deltaTime;

               // Debug.Log("Flower grow " + flower.Size + " " + flower.Lifetime);
            }
        }
    }
}
