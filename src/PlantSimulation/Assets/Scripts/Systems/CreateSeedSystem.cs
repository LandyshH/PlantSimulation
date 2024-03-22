using Assets.Scripts.Enum;
using Assets.Scripts.Providers;
using Leopotam.Ecs;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public sealed class CreateSeedSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld;
        private StaticData staticData;

        public void Init()
        {
            if (staticData.SeedSpawned)
            {
                return;
            }

            Debug.Log("Spawn seed");

            staticData.SeedSpawned = true;

            var position = new Vector3();

            var plant = Object.Instantiate(staticData.PlantPrefab, position, Quaternion.identity);

            var seedEntity = _ecsWorld.NewEntity();
            seedEntity.Replace(new SeedComponent
            {
                Position = position,
                Size = 10,
                Stage = SeedGrowthStage.Zygote,
                Lifetime = 0,
                gameObject = plant
            }) ; 
            //ref var seedComponent = ref seedEntity.Get<SeedComponent>();


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
