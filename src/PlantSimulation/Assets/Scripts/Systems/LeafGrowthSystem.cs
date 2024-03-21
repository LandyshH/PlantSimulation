using Assets.Scripts.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public sealed class LeafGrowthSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private EnvironmentSettings environment;

        EcsFilter<LeafComponent> _leafFilter;
        EcsFilter<StemComponent> _stemFilter;

        public void Run()
        {
            ref var stem = ref _stemFilter.Get1(0);

            foreach (var i in _leafFilter)
            {
                /*if (stem.GrowthStage == Enum.PlantGrowthStage.Embryonic || stem.GrowthStage == Enum.PlantGrowthStage.Senile)
                {
                    continue;
                }*/

                ref var leaf = ref _leafFilter.Get1(i);

                //Debug.Log("Leaf grow " + leaf.Size + " " + leaf.Lifetime);

                //ref var environment = ref _filter.Get2(i);

                if (environment.Temperature != Enum.Temperature.Optimal)
                {
                    leaf.Size += GrowthRateCalculator.CalculateGrowthRate(environment) * Time.deltaTime / 10;
                    leaf.Lifetime += 10 * Time.deltaTime;
                    continue;
                }

                leaf.Size += GrowthRateCalculator.CalculateGrowthRate(environment) * Time.deltaTime;
                leaf.Lifetime += 10 * Time.deltaTime;

                //Debug.Log("Leaf grow " + leaf.Size + " " + leaf.Lifetime);
            }
        }
    }
}
