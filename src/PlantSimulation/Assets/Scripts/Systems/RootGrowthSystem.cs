using Assets.Scripts.Enum;
using Assets.Scripts.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public sealed class RootGrowthSystem : IEcsRunSystem
    {
        private EnvironmentSettings environment;
        private StaticData staticData;

        EcsFilter<RootComponent> _rootFilter;

        public void Run()
        {
            if (!staticData.SeedSpawned || !staticData.RootSpawned)
            {
                return;
            }

            foreach (var i in _rootFilter)
            {
                ref var root = ref _rootFilter.Get1(i);

                //Debug.Log("Root grow " + i + " " + staticData.PlantGrowthStage + " " + root.Size + " " + root.Lifetime);

                if (environment.Oxygen == Oxygen.Lack && staticData.PlantGrowthStage == PlantGrowthStage.Juvenile)
                {
                    root.IsOxygenLack = true;
                }

                switch (staticData.PlantGrowthStage)
                {
                    case PlantGrowthStage.Juvenile:
                        root.Lifetime += Time.deltaTime * 10;
                        root.Size += GrowthRateCalculator.CalculateGrowthRate(environment) * Time.deltaTime;

                        if (root.Lifetime >= 100f)
                        {
                            staticData.PlantGrowthStage = PlantGrowthStage.MaturityAndReproduction;
                        }

                        break;

                    case PlantGrowthStage.MaturityAndReproduction:
                        root.Lifetime += Time.deltaTime * 10;
                        root.Size += GrowthRateCalculator.CalculateGrowthRate(environment) * Time.deltaTime;

                        if (root.Lifetime >= 400f)
                        {
                            staticData.PlantGrowthStage = PlantGrowthStage.Senile;
                        }

                        break;
                    case PlantGrowthStage.Senile:
                        root.Lifetime += Time.deltaTime * 10; 
                        break;
                }

              //  Debug.Log("Root grow " + root.Size + " " + root.Lifetime);
            }
        }
    }
}
