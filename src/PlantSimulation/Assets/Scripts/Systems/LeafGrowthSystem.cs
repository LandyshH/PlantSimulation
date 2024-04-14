using Assets.Scripts.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public sealed class LeafGrowthSystem : IEcsRunSystem
    {
        private EnvironmentSettings environment;
        private StaticData _staticData;

        EcsFilter<LeafComponent> _leafFilter;
        EcsFilter<StemComponent> _stemFilter;

        public void Run()
        {
            if (_staticData.PlantGrowthStage == Enum.PlantGrowthStage.Senile || _staticData.GoToNextStage)
            {
                return;
            }

            ref var stem = ref _stemFilter.Get1(0);

            foreach (var i in _leafFilter)
            {
                ref var leaf = ref _leafFilter.Get1(i);

                if (i == 0 || i == 1)
                {
                    if (_staticData.PlantGrowthStage == Enum.PlantGrowthStage.Juvenile)
                    {
                        leaf.Size -= 0.1f;
                        if (leaf.Size <= 0)
                        {
                            leaf.Size = 0;
                        }
                    }
                }
                else
                {
                    if (environment.Temperature == Enum.Temperature.Max)
                    {
                        leaf.Size -= 0.1f;

                        if (leaf.Size <= 0)
                        {
                            leaf.Size = 0;
                        }

                        leaf.Lifetime += Time.deltaTime;
                    }
                    else
                    {
                        leaf.Size += GrowthRateCalculator.CalculateGrowthRate(environment) * 0.01f;
                    }
                }
                
                leaf.Lifetime += Time.deltaTime;
                Debug.Log("Leaf: " + leaf.Lifetime);
            }
        }
    }
}
