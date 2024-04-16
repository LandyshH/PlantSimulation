using Assets.Scripts.ProceduralGeneration.Sunflower.ScriptableObjects;
using Assets.Scripts.Tags;
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
        private StaticData staticData;
        private EnvironmentSettings environment;

        public void Init()
        {
            ref var rootComponent = ref _filter.Get1(0);

            var stemEntity = _ecsWorld.NewEntity();
            ref var stem = ref stemEntity.Get<StemComponent>();

            stem.Lifetime = 0;
            stem.Position = rootComponent.Position;
            stem.Height = 0;
            stem.MaxHeight = 15f;
            stem.Width = 0.1f;
            stem.MaxWidth = 3f;
            staticData.StemHeightDiff = 0;

            var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();

            var stemGO = Object.Instantiate(sunflowerObjects.StemPrefab, plant.transform);
            stemGO.name = "Stem";
            stemGO.transform.localScale = new Vector3(stem.Width, stem.Width, stem.Height);
            stemGO.transform.localPosition = Vector3.zero;

            for (int i = 0; i < 2; i++)
            {
                float angle = i * (360f / 2);
                Quaternion rotation = Quaternion.Euler(235f, angle, 0f);

                Vector3 leafPosition = stemGO.transform.position + Vector3.up * stemGO.transform.localScale.z * 0.25f;
                GameObject leafGO = Object.Instantiate(sunflowerObjects.LeafPrefab, plant.transform);

                leafGO.transform.localRotation = rotation;
                leafGO.transform.position = leafPosition;

                var leafSize = 10f;

                leafGO.transform.localScale = new Vector3(leafSize, leafSize, 0.5f);
                leafGO.name = "Sprout Leaf " + i;

                var leafEntity = _ecsWorld.NewEntity();
                ref var leaf = ref leafEntity.Get<LeafComponent>();
             
                leafEntity.Get<SproutTag>();

                leaf.Height = leafSize;
                leaf.Width = leafSize;
                leaf.LeafGO = leafGO;
            }

            stem.stemGO = stemGO;

            staticData.leafPositions.Clear();
        }
    }
}
