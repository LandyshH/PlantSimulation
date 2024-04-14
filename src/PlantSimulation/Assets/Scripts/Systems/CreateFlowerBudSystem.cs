using Assets.Scripts.ProceduralGeneration.Sunflower.ScriptableObjects;
using Leopotam.Ecs;
using System.Linq;
using UnityEngine;

public sealed class CreateFlowerBudSystem : IEcsInitSystem
{
    private readonly EcsWorld _ecsWorld;

    EcsFilter<StemComponent> _filter;

    public SunflowerObjects sunflowerObjects;

    public void Init()
    {
        ref var stemComponent = ref _filter.Get1(0);

        var flowerBudEntity = _ecsWorld.NewEntity();
        ref var bud = ref flowerBudEntity.Get<FlowerBudComponent>();

        bud.Lifetime = 0;
        bud.Size = 0;

        var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();

        var budGO = Object.Instantiate(sunflowerObjects.BudPrefab, plant.transform);

        budGO.transform.position = stemComponent.stemGO.transform.position + 
            Vector3.up * stemComponent.stemGO.transform.localScale.z * 0.25f;

        budGO.transform.localScale = new Vector3(bud.Size, bud.Size, bud.Size);
        budGO.name = "Flower Bud";

        bud.FlowerBudGO = budGO;
    }
}
