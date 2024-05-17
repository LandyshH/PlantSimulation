using Leopotam.Ecs;
using UnityEngine;

public sealed class StrawberryAnimationSystem : IEcsRunSystem
{
    private EnvironmentSettings environment;
    private StaticData staticData;
    private StrawberryObjects strawberryPrefabs;

    EcsFilter<FlowerComponent> _filter;

    public void Run()
    {
        if (staticData.GoToNextStage) return;

        foreach (var i in _filter)
        {
            ref var component = ref _filter.Get1(i);

            if (component.FlowerGO == null) continue;

            if (component.ChangedToRedFruit) continue;

            Debug.Log("flower lifetime: " + component.Lifetime);

            if (component.IsBud && component.Lifetime > 5f)
            {
                var transform = component.FlowerGO.transform;
                var scale = component.FlowerGO.transform.localScale;
                Object.Destroy(component.FlowerGO);

                var flower = Object.Instantiate(strawberryPrefabs.FlowerPrefab, transform.position,
                    transform.rotation, transform.parent);
                flower.transform.localScale = scale;

                component.FlowerGO = flower;

                component.ChangedToFlower = true;
                component.IsBud = false;
            }
            else if (component.ChangedToFlower && component.Lifetime > 10f)
            {
                var transform = component.FlowerGO.transform;
                var scale = component.FlowerGO.transform.localScale;
                Object.Destroy(component.FlowerGO);

                var flower = Object.Instantiate(strawberryPrefabs.GreenFruitPrefab, transform.position,
                    transform.rotation, transform.parent);
                flower.transform.localScale = scale;

                component.FlowerGO = flower;

                component.ChangedToGreenFruit = true;
                component.ChangedToFlower = false;
            }
            else if (component.ChangedToGreenFruit && component.Lifetime > 15f)
            {
                var transform = component.FlowerGO.transform;
                var scale = component.FlowerGO.transform.localScale;
                Object.Destroy(component.FlowerGO);

                var flower = Object.Instantiate(strawberryPrefabs.WhiteFruitPrefab, transform.position,
                    transform.rotation, transform.parent);
                flower.transform.localScale = scale;

                component.FlowerGO = flower;

                component.ChangedToWhiteFruit = true;
                component.ChangedToGreenFruit = false;
            }
            else if (component.ChangedToWhiteFruit && component.Lifetime > 20f)
            {
                var transform = component.FlowerGO.transform;
                var scale = component.FlowerGO.transform.localScale;
                Object.Destroy(component.FlowerGO);

                var flower = Object.Instantiate(strawberryPrefabs.RedFruitPrefab, transform.position,
                    transform.rotation, transform.parent);
                flower.transform.localScale = scale;

                component.FlowerGO = flower;

                component.ChangedToRedFruit = true;
                component.ChangedToWhiteFruit = false;
            }


            component.Lifetime += Time.deltaTime;
        }
    }
}
