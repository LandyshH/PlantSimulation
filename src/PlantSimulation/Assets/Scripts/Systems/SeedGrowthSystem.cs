using Assets.Scripts.Enum;
using Assets.Scripts.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public sealed class SeedGrowthSystem : IEcsRunSystem
    {
        private EnvironmentSettings environment;
        private StaticData staticData;

        EcsFilter<SeedComponent> _filter;

        public void Run()
        {
            if (staticData.PlantGrowthStage != PlantGrowthStage.Embryonic)
            {
                return;
            }

            if (staticData.GoToNextStage)
            {
                return;
            }

            foreach (var i in _filter)
            {
                ref var seedComponent = ref _filter.Get1(i);
                
                
                Debug.Log("Doing " + " " + seedComponent.Stage + " " + seedComponent.Lifetime + " " + seedComponent.Size);

                switch (seedComponent.Stage)
                {
                    case SeedGrowthStage.Zygote:
    
                        seedComponent.Size += GrowthRateCalculator.CalculateGrowthRate(environment) * Time.deltaTime * 5;
                        seedComponent.Lifetime += Time.deltaTime;

                        if (seedComponent.Size > 15f)
                        {
                            seedComponent.Stage = SeedGrowthStage.Proembryo;
                        }

                        break;

                    case SeedGrowthStage.Proembryo:

                        seedComponent.Size += GrowthRateCalculator.CalculateGrowthRate(environment) * Time.deltaTime * 5;
                        seedComponent.Lifetime += Time.deltaTime;

                        if (seedComponent.Size > 20f)
                        {
                            seedComponent.Stage = SeedGrowthStage.Globular;
                        }
                        break;
                    case SeedGrowthStage.Globular:
                        seedComponent.Size += GrowthRateCalculator.CalculateGrowthRate(environment) * Time.deltaTime * 5;
                        seedComponent.Lifetime += Time.deltaTime;

                        if (seedComponent.Size > 25f)
                        {
                            seedComponent.Stage = SeedGrowthStage.HeartShaped;

                        }
                        break;
                    case SeedGrowthStage.HeartShaped:
                        seedComponent.Size += GrowthRateCalculator.CalculateGrowthRate(environment) * Time.deltaTime * 5;
                        seedComponent.Lifetime += Time.deltaTime;

                        if (seedComponent.Size > 30f)
                        {
                            seedComponent.Stage = SeedGrowthStage.TorpedoShaped;
                        }
                        break;
                    case SeedGrowthStage.TorpedoShaped:
                        seedComponent.Lifetime += Time.deltaTime;
                        staticData.GoToNextStage = true;
                        break;
                }
            }
        }
    }
}
