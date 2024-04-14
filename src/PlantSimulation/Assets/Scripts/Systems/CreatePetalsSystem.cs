using Assets.Scripts.ProceduralGeneration.Sunflower.ScriptableObjects;
using Leopotam.Ecs;
using System.Linq;
using UnityEngine;

public sealed class CreatePetalsSystem : IEcsInitSystem
{
    private readonly EcsWorld _ecsWorld;
    private readonly StaticData _staticData;
    private readonly SunflowerObjects _sunflowerObjects;

    private readonly EcsFilter<FlowerBudComponent> _filter;

    public void Init()
    {
        ref var bud = ref _filter.Get1(0);

        var petalCount = _staticData.PetalCount;
        for (int i = 0; i < petalCount; i++)
        {
            var petalEntity = _ecsWorld.NewEntity();
            ref var petal = ref petalEntity.Get<PetalComponent>();
            petal.Size = 0;

            float angle = i * (360f / petalCount);
            Quaternion rotation = Quaternion.Euler(0f, 90f, angle);

            var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();

            Vector3 petalPosition = bud.FlowerBudGO.transform.position 
                + rotation * Vector3.right * bud.Size 
                + Vector3.left * 0.05f;
            GameObject petalGO = Object.Instantiate(_sunflowerObjects.PetalPrefab, plant.transform);

            petalGO.transform.localScale = new Vector3(petal.Size, petal.Size, petal.Size);
            petalGO.transform.localRotation = rotation;
            petalGO.transform.position = petalPosition;
            petalGO.name = "Petal " + i;

            petal.PetalGO = petalGO;
        }
    }
}
