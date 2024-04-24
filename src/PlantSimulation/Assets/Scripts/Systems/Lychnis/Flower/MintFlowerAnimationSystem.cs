using Leopotam.Ecs;
using UnityEngine;

namespace Assets.Scripts.Systems.Lychnis.Flower
{
    public class MintFlowerAnimationSystem : IEcsRunSystem
    {
        private EcsFilter<FlowerComponent> _filter;
        private StaticData _staticData;
        private MintPrefabs _prefabs;

        public void Run()
        {
            if (_staticData.PlantGrowthStage != Enum.PlantGrowthStage.MaturityAndReproduction)
            {
                return;
            }

            foreach (var i in _filter)
            {
                ref var component = ref _filter.Get1(i);

                if (component.FlowerGO == null || component.ChangedToFlower) continue;

                var transform = component.FlowerGO.transform;
                var scale = component.FlowerGO.transform.localScale;
                Object.Destroy(component.FlowerGO);

                var flower = Object.Instantiate(_prefabs.FlowerPrefab, transform.position, transform.rotation, null);
                flower.transform.localScale = scale;

                component.FlowerGO = flower;

                component.ChangedToFlower = true;
            }
        }
    }
}
