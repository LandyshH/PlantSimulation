using Leopotam.Ecs;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public sealed class CreateRootSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld;
        private StaticData staticData;

        public void Init()
        {
            Debug.Log("Create root");
            var rootEntity = _ecsWorld.NewEntity();
            ref var root = ref rootEntity.Get<RootComponent>();

            root.Position = new Vector3();
            root.Lifetime = 0;
        }
    }
}
