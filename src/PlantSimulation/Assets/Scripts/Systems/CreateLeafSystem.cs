using Assets.Scripts.ProceduralGeneration.Sunflower.ScriptableObjects;
using Leopotam.Ecs;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public sealed class CreateLeafSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld;
        private EnvironmentSettings environment;
        private readonly StaticData _staticData;
        private readonly SunflowerObjects _sunflowerObjects;

        private readonly EcsFilter<StemComponent>
           .Exclude<BlockCreateDuration> _stemFilter;

        public void Run()
        {

            if (_staticData.PlantGrowthStage == Enum.PlantGrowthStage.Senile
            || _staticData.PlantGrowthStage == Enum.PlantGrowthStage.Embryonic
            || _staticData.GoToNextStage)
            {
                return;
            }

            if (_staticData.leafPositions.Count >= 28)
            {
                return;
            }

            foreach (var i in _stemFilter)
            {
                ref var stemEntity = ref _stemFilter.GetEntity(i);
                ref var stemComponent = ref _stemFilter.Get1(i);

                if (stemComponent.Height < 2f)
                {
                    return;
                }

                var angle = Random.Range(0f, 360f);
                var rotation = Quaternion.Euler(-60f, angle, 0f);

                var minHeight = stemComponent.stemGO.transform.localScale.z * 0.1f;
                var maxHeight = stemComponent.stemGO.transform.localScale.z * 0.22f;

                bool tooClose = false;
                var leafPosition = stemComponent.stemGO.transform.position + Vector3.up * Random.Range(minHeight, maxHeight);

                foreach (Vector3 existingPosition in _staticData.leafPositions)
                {
                    var minDistance = environment.Light == Enum.LightColor.Darkness ? 0.3f : 0.1f;
                    if (Vector3.Distance(leafPosition, existingPosition) < minDistance)
                    {
                        tooClose = true;
                    }
                }

                if (tooClose)
                {
                    return;
                }

                _staticData.leafPositions.Add(leafPosition);

                var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();
                var leafEntity = _ecsWorld.NewEntity();
                ref var leaf = ref leafEntity.Get<LeafComponent>();
                leaf.Lifetime = 0;
                leaf.Size = 0;
                leaf.maxSize = 12f;
                leaf.Height = 0;
                leaf.Width = 0;
                leaf.MaxHeight = 12f;
                leaf.MaxWidth = 12f;

                var leafGO = Object.Instantiate(_sunflowerObjects.LeafPrefab, plant.transform);
                leafGO.transform.localRotation = rotation;
                leafGO.transform.position = leafPosition;
                leafGO.transform.localScale = new Vector3(leaf.Width, leaf.Height, 0.5f);
                leafGO.name = "Leaf";

                leaf.LeafGO = leafGO;

                if (environment.Minerals == Enum.Minerals.Lack || environment.Temperature == Enum.Temperature.Min || environment.Water == Enum.Water.Lack)
                {
                    stemEntity.Get<BlockCreateDuration>().Timer = 4f;
                }
                else
                {
                    stemEntity.Get<BlockCreateDuration>().Timer = 2f;
                }
            }
        }

        
        private EcsEntity CreateFlower(Vector3 position)
        {
            var flowerEntity = _ecsWorld.NewEntity();
            ref var leaf = ref flowerEntity.Get<FlowerComponent>();

            leaf.Lifetime = 0;
            leaf.Size = 10;

            leaf.Position = position;

            return flowerEntity;
        }
    }
}

