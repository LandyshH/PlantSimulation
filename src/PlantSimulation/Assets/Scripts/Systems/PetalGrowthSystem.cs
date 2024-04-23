using Leopotam.Ecs;
using System.Linq;
using UnityEngine;


public sealed class PetalGrowthSystem : IEcsRunSystem
{
    private EnvironmentSettings environment;
    private StaticData _staticData;

    EcsFilter<PetalComponent> _filter;
    private readonly EcsFilter<FlowerBudComponent> _budFilter;

    public void Run()
    {
        ref var bud = ref _budFilter.Get1(0);

        foreach (var i in _filter)
        {
            ref var petal = ref _filter.Get1(i);

            if (_staticData.PlantGrowthStage != Assets.Scripts.Enum.PlantGrowthStage.MaturityAndReproduction
                && _staticData.PlantGrowthStage != Assets.Scripts.Enum.PlantGrowthStage.Senile)
            {
                return;
            }

            petal.Size = 8f;

            if (environment.Temperature == Assets.Scripts.Enum.Temperature.Max)
            {
                petal.Size = 6f;

                if (petal.Lifetime > 10)
                {
                    petal.Size -= 2f;
                }
            }

            if (environment.Temperature == Assets.Scripts.Enum.Temperature.Max)
            {
                petal.Size = 5f;

                if (petal.Lifetime > 10)
                {
                    petal.Size -= 2f;
                }
            }


            var petalCount = _staticData.PetalCount;

            float angle = i * (360f / petalCount);
            Quaternion rotation = Quaternion.Euler(0f, 90f, angle);

            var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();

            Vector3 petalPosition = bud.FlowerBudGO.transform.position
                + rotation * Vector3.right * 0.12f
                + Vector3.left * 0.02f;

            petal.PetalGO.transform.localRotation = rotation;
            petal.PetalGO.transform.position = petalPosition;

            petal.Lifetime += Time.deltaTime;
        }
    }
}
