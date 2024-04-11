using Assets.Scripts.ProceduralGeneration.Sunflower.ScriptableObjects;
using Leopotam.Ecs;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public sealed class CreateStemSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld;

        EcsFilter<RootComponent> _filter;

        public SunflowerObjects sunflowerObjects;
        //private ProceduralSunflower proceduralSunflower;

        public void Init()
        {
            ref var rootComponent = ref _filter.Get1(0);

            var stemEntity = _ecsWorld.NewEntity();
            ref var stem = ref stemEntity.Get<StemComponent>();

            stem.Lifetime = 0;
            stem.Position = rootComponent.Position;
            stem.Height = 2;
            stem.Width = 3;

            var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();

            //var stemGO = proceduralSunflower.CreateStem(stem.Width, stem.Height, plant.transform);

            var stemGO = Object.Instantiate(sunflowerObjects.StemPrefab, plant.transform);
            stemGO.name = "Stem";
            stemGO.transform.localScale = new Vector3(stem.Width, stem.Width, stem.Height);
            stemGO.transform.localPosition = Vector3.zero;

            stem.stemGO = stemGO;
        }
    }
}
