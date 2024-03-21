using Leopotam.Ecs;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public sealed class CreateRootSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld = null;

        EcsFilter<SeedComponent> _filter;

        public void Init()
        {
            Debug.Log("Create root");
            var rootEntity = _ecsWorld.NewEntity();
            ref var root = ref rootEntity.Get<RootComponent>();

            ref var seedComponent = ref _filter.Get1(0);
            //ref var environment = ref _filter.Get2(0);

            root.Position = seedComponent.Position;
            root.Lifetime = 0;
            root.GrowthStage = Enum.PlantGrowthStage.Juvenile;
        }
    }
}
