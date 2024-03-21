using Assets.Scripts.Enum;
using Assets.Scripts.Services;
using Leopotam.Ecs;
using System;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public sealed class SeedGrowthSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private EnvironmentSettings environment;
        private StaticData staticData;

        EcsFilter<SeedComponent> _filter;

        public void Run()
        {
            if (staticData.SeedSpawned) return;

            //foreach (var i in _filter)
            // {
            ref var seedComponent = ref _filter.Get1(0);
            ref var seedEntity = ref _filter.GetEntity(0);

            //var destroyed = false;
            // ref var environment = ref _filter.Get2(i);

            Debug.Log("Doing " + " " + seedComponent.Stage + " " + seedComponent.Lifetime + " " + seedComponent.Size);

            //while (!destroyed)
            //{
                switch (seedComponent.Stage)
                {
                    case SeedGrowthStage.Zygote:
                        //while (seedComponent.Size <= 15f)
                        //{
                            seedComponent.Size += GrowthRateCalculator.CalculateGrowthRate(environment) * Time.deltaTime * 5;
                            seedComponent.Lifetime += Time.deltaTime;
                            
                        //    Debug.Log("Doing " + " " + seedComponent.Stage + " " + seedComponent.Lifetime + " " + seedComponent.Size);
                        //}

                        if (seedComponent.Size > 15f)
                        {
                            seedComponent.Stage = SeedGrowthStage.Proembryo;
                        }

                        break;

                    case SeedGrowthStage.Proembryo:
                       // while (seedComponent.Size <= 20f)
                        //{
                            seedComponent.Size += GrowthRateCalculator.CalculateGrowthRate(environment) * Time.deltaTime * 5;
                            seedComponent.Lifetime += Time.deltaTime;
                       //     Debug.Log("Doing " + " " + seedComponent.Stage + " " + seedComponent.Lifetime + " " + seedComponent.Size);
                        //}

                        if (seedComponent.Size > 20f)
                        {
                            seedComponent.Stage = SeedGrowthStage.Globular;
                        }
                        break;
                    case SeedGrowthStage.Globular:
                        //while (seedComponent.Size <= 25f)
                        //{
                            seedComponent.Size += GrowthRateCalculator.CalculateGrowthRate(environment) * Time.deltaTime * 5;
                            seedComponent.Lifetime += Time.deltaTime;
                       //     Debug.Log("Doing " + " " + seedComponent.Stage + " " + seedComponent.Lifetime + " " + seedComponent.Size);
                       // }

                        if (seedComponent.Size > 25f)
                        {
                            seedComponent.Stage = SeedGrowthStage.HeartShaped;

                        }
                        break;
                    case SeedGrowthStage.HeartShaped:
                        //while (seedComponent.Size <= 30f)
                        //{
                            seedComponent.Size += GrowthRateCalculator.CalculateGrowthRate(environment) * Time.deltaTime * 5;
                            seedComponent.Lifetime += Time.deltaTime;
                          //  Debug.Log("Doing " + " " + seedComponent.Stage + " " + seedComponent.Lifetime + " " + seedComponent.Size);
                        //}

                        if (seedComponent.Size > 30f)
                        {
                            seedComponent.Stage = SeedGrowthStage.TorpedoShaped;
                        }
                        break;
                    case SeedGrowthStage.TorpedoShaped:
                        seedComponent.Lifetime += Time.deltaTime;
                        Debug.Log("Seed: end " + "Lifetime: " + seedComponent.Lifetime + " Size: " + seedComponent.Size);
                        seedEntity.Destroy();
                        staticData.SeedSpawned = true;

                    //destroyed = true;
                    break;

               // }
            }
        }
    }
}
