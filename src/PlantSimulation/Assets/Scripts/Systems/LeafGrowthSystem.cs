using Assets.Scripts.Services;
using Assets.Scripts.Tags;
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
            if (_staticData.GoToNextStage)
            {
                return;
            }

            ref var stem = ref _stemFilter.Get1(0);

            foreach (var i in _leafFilter)
            {
                ref var leaf = ref _leafFilter.Get1(i);
                ref var leafEntity = ref _leafFilter.GetEntity(i);

                if (leaf.Lifetime >= 30) 
                {
                    leaf.Height -= 4f * Time.deltaTime;
                    leaf.Width -= 4f * Time.deltaTime;
                    if (leaf.Width <= 0)
                    {
                        leaf.LeafGO.SetActive(false);
                        leafEntity.Destroy();
                    }

                    return;
                }

                leaf.Lifetime += Time.deltaTime;

                if (leaf.Height >= leaf.MaxHeight && !leafEntity.Has<SproutTag>())
                {
                    continue;
                }

                if (leafEntity.Has<SproutTag>())
                {
                    if (_staticData.PlantGrowthStage == Enum.PlantGrowthStage.Juvenile)
                    {
                        leaf.Height -= 5f * Time.deltaTime;
                        leaf.Width -= 5f * Time.deltaTime;

                        if (leaf.Width <= 0)
                        {
                            leaf.LeafGO.SetActive(false);
                            leafEntity.Destroy();
                        }
                    }
                }
                else
                {
                    if (environment.Temperature == Enum.Temperature.Max && leaf.Lifetime >= 5)
                    {
                        leaf.Width -= 3f * Time.deltaTime;
                        leaf.Height -= 5f * Time.deltaTime;

                        if (leaf.Width <= 0)
                        {
                            leaf.LeafGO.SetActive(false);
                            leafEntity.Destroy();
                        }
                    }
                    else
                    {
                        if (leaf.Height < leaf.MaxHeight)
                        {
                            if (environment.Temperature == Enum.Temperature.Max)
                            {
                                leaf.Height += GrowthRateCalculator.CalculateGrowthRate(environment) * 6 * Time.deltaTime;
                                leaf.Width += GrowthRateCalculator.CalculateGrowthRate(environment) * 3 * Time.deltaTime;
                            }
                            else if (environment.Temperature == Enum.Temperature.Min)
                            {
                                leaf.Height += GrowthRateCalculator.CalculateGrowthRate(environment) * 6 * Time.deltaTime;
                                leaf.Width += GrowthRateCalculator.CalculateGrowthRate(environment) * 2 * Time.deltaTime;
                            }
                            else
                            {
                                //Debug.Log("RATE: " + GrowthRateCalculator.CalculateGrowthRate(environment));
                                leaf.Height += GrowthRateCalculator.CalculateGrowthRate(environment) * 5 * Time.deltaTime;
                                leaf.Width += GrowthRateCalculator.CalculateGrowthRate(environment) * 5 * Time.deltaTime;
                            }
                        }
                    }
                }
            }
        }
    }
}
