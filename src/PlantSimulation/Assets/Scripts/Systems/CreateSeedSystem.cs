using Assets.Scripts.Enum;
using Assets.Scripts.Providers;
using Assets.Scripts.Enum;
using Assets.Scripts.Providers;
using Assets.Scripts.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public sealed class CreateSeedSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private StaticData staticData;

        public void Init()
        {
            var seedEntity = _ecsWorld.NewEntity();
            ref var seedComponent = ref seedEntity.Get<SeedComponent>();

            seedComponent.Position = new Vector3();
            seedComponent.Size = 10;
            seedComponent.Stage = 0;
            seedComponent.Lifetime = 0;

            var plant = Object.Instantiate(staticData.PlantPrefab, seedComponent.Position, Quaternion.identity);
            plant.AddComponent<SeedProvider>();
            seedComponent.gameObject = plant;
            Debug.Log("Spawn seed");

           // ref var environment = ref seedEntity.Get<EnvironmentComponent>();

            /* environment.Light = LightColor.Darkness;
            environment.Water = Water.Optimal;
            environment.Temperature = Temperature.Min;
            environment.CarbonDioxide = CarbonDioxide.Optimal;
            environment.Oxygen = Oxygen.Optimal;
            environment.Minerals = Minerals.Optimal; */
        }
    }
}
