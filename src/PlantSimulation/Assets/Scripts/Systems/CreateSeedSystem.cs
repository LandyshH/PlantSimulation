using Assets.Scripts.Enum;
using Leopotam.Ecs;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public sealed class CreateSeedSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld;
        private StaticData staticData;
        private readonly UI ui;
        private EnvironmentSettings environment;

        public void Init()
        {
            //Debug.Log("Spawn seed");

            var position = new Vector3();

           // var plant = Object.Instantiate(staticData.SeedPrefab, position, Quaternion.identity);

            var seedEntity = _ecsWorld.NewEntity();
            seedEntity.Replace(new SeedComponent
            {
                Position = position,
                Size = 10,
                Stage = SeedGrowthStage.Zygote,
                Lifetime = 0,
               // gameObject = plant
            }) ;

            staticData.PlantGrowthStage = PlantGrowthStage.Embryonic;
            staticData.GoToNextStage = false;
            ui.environmentWindowScreen.SetActive(false);

            environment.Light = LightColor.Sun;
            environment.Water = Water.Optimal;
            environment.Temperature = Temperature.Optimal;
            environment.CarbonDioxide = CarbonDioxide.Optimal;
            environment.Oxygen = Oxygen.Optimal;
            environment.Minerals = Minerals.Optimal;
        }
    }
}
