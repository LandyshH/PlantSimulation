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
            if (staticData.PlantGrowthStage == PlantGrowthStage.Embryonic)
            {
                return;
            }

            if (staticData.GoToNextStage)
            {
                return;
            }

            foreach (var i in _rootFilter)
            {
                ref var root = ref _rootFilter.Get1(i);

                if (environment.Oxygen == Oxygen.Lack && staticData.PlantGrowthStage == PlantGrowthStage.Juvenile)
                {
                    root.IsOxygenLack = true;
                }

                switch (staticData.PlantGrowthStage)
                {
                    case PlantGrowthStage.Juvenile:
                        if (root.Lifetime >= 20f)
                        {
                            staticData.GoToNextStage = true;
                            break;
                        }

                        root.Lifetime += Time.deltaTime;
                        root.Size += GrowthRateCalculator.CalculateGrowthRate(environment);

                        break;

                    case PlantGrowthStage.MaturityAndReproduction:
                        if (root.Lifetime >= 40f)
                        {
                            staticData.GoToNextStage = true;
                            break;
                        }

                        root.Lifetime += Time.deltaTime;
                        root.Size += GrowthRateCalculator.CalculateGrowthRate(environment);

                        break;
                    case PlantGrowthStage.Senile:
                        staticData.GoToNextStage = false;
                        root.Lifetime += Time.deltaTime; 
                        break;
                }
            }
        }
    }
}
