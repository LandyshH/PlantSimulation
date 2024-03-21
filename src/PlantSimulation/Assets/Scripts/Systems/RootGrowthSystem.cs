using Assets.Scripts.Enum;
using Assets.Scripts.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public sealed class RootGrowthSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private EnvironmentSettings environment;
        private StaticData staticData;

        EcsFilter<RootComponent> _rootFilter;

        public void Run()
        {
            //ref var stem = ref _stemFilter.Get1(0);

            foreach (var i in _rootFilter)
            {
                if (!staticData.SeedSpawned)
                {
                    return;
                }

                ref var root = ref _rootFilter.Get1(i);

                Debug.Log("Root grow " + i + " " + root.GrowthStage + " " + root.Size + " " + root.Lifetime);
                /* if (stem.GrowthStage == PlantGrowthStage.Embryonic)
                 {
                     continue;
                 }*/

                //ref var environment = ref _filter.Get2(i);

               // root.Position = new Vector3();

                //root.Size += GrowthRateCalculator.CalculateGrowthRate(environment) * Time.deltaTime;
                //root.Lifetime += Time.deltaTime;

                if (environment.Oxygen == Oxygen.Lack && root.GrowthStage == PlantGrowthStage.Juvenile)
                {
                    root.IsOxygenLack = true;
                }

                switch (root.GrowthStage)
                {
                    case PlantGrowthStage.Juvenile:
                        root.Lifetime += Time.deltaTime * 10;
                        root.Size += GrowthRateCalculator.CalculateGrowthRate(environment) * Time.deltaTime;

                        if (root.Lifetime >= 100f)
                        {
                            root.GrowthStage = PlantGrowthStage.MaturityAndReproduction;
                        }

                        break;

                    case PlantGrowthStage.MaturityAndReproduction:
                        root.Lifetime += Time.deltaTime * 10;
                        root.Size += GrowthRateCalculator.CalculateGrowthRate(environment) * Time.deltaTime;

                        if (root.Lifetime >= 400f)
                        {
                            root.GrowthStage = PlantGrowthStage.Senile;
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
