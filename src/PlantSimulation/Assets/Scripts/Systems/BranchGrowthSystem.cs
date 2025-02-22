﻿using Assets.Scripts.Enum;
using Assets.Scripts.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public sealed class BranchGrowthSystem : IEcsRunSystem
    {
        private EnvironmentSettings environment;

        EcsFilter<BranchComponent> _branchFilter;

        public void Run()
        {
            foreach (var i in _branchFilter)
            {
                ref var branch = ref _branchFilter.Get1(i);

                branch.Lifetime += 10 * Time.deltaTime;

                if (environment.CarbonDioxide != CarbonDioxide.Excess)
                {
                    if (environment.Temperature == Temperature.Max || environment.Water == Water.Lack)
                    {
                        branch.Height += GrowthRateCalculator.CalculateGrowthRate(environment) / 15;

                        if (environment.Temperature == Temperature.Max)
                        {
                            branch.Width -= 1;
                        }

                        if (environment.Water == Water.Lack)
                        {
                            branch.Width -= 1;
                        }

                        continue;
                    }

                    if (environment.Temperature == Temperature.Min
                        || environment.Oxygen == Oxygen.Lack || environment.Oxygen == Oxygen.Excess
                        || environment.Light == LightColor.Darkness)
                    {
                        branch.Height += GrowthRateCalculator.CalculateGrowthRate(environment) / 15;
                        branch.Width += GrowthRateCalculator.CalculateGrowthRate(environment) / 10;
                        continue;
                    }

                    if (environment.Light == LightColor.Blue)
                    {
                        branch.Width += GrowthRateCalculator.CalculateGrowthRate(environment) / 10;
                    }

                    if (environment.Light == LightColor.Red)
                    {
                        branch.Height += GrowthRateCalculator.CalculateGrowthRate(environment) / 15;
                    }

                    branch.Height += GrowthRateCalculator.CalculateGrowthRate(environment) / 15;
                    branch.Width += GrowthRateCalculator.CalculateGrowthRate(environment) / 10;
                }

            }
        }
    }
}
