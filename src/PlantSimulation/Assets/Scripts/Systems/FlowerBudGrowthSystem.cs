using Assets.Scripts.Services;
using Leopotam.Ecs;
using UnityEngine;

public sealed class FlowerBudGrowthSystem : IEcsRunSystem
{
    private EnvironmentSettings environment;
    private readonly StaticData staticData;

    EcsFilter<FlowerBudComponent> filter;

    public void Run()
    {
        if (staticData.PlantGrowthStage != Assets.Scripts.Enum.PlantGrowthStage.Juvenile)
        {
            return;
        }

        foreach (var i in filter)
        {
            ref var flowerBud = ref filter.Get1(i);
            if (flowerBud.Size >= 15f)
            {
                return;
            }

            var growthRate = 0.5f;
            flowerBud.Size += growthRate * 0.09f;
        }
    }
}
