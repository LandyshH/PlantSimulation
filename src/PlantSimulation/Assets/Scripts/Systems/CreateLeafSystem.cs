using Assets.Scripts.ProceduralGeneration.Sunflower.ScriptableObjects;
using Leopotam.Ecs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

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


            foreach (var i in _stemFilter)
            {
                Debug.Log("Create Leaf");

                ref var stemEntity = ref _stemFilter.GetEntity(i);
                ref var stemComponent = ref _stemFilter.Get1(i);

                var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();
                var leafEntity = _ecsWorld.NewEntity();
                ref var leaf = ref leafEntity.Get<LeafComponent>();
                leaf.Lifetime = 0;
                leaf.Size = 10;

                var angle = Random.Range(0f, 360f);
                var rotation = Quaternion.Euler(-60f, angle, 0f);

                var minHeight = stemComponent.stemGO.transform.localScale.z * 0.1f;
                var maxHeight = stemComponent.stemGO.transform.localScale.z * 0.20f;

                var leafPosition = GetLeafPosition(stemComponent, minHeight, maxHeight);


                var leafGO = Object.Instantiate(_sunflowerObjects.LeafPrefab, plant.transform);
                leafGO.transform.localRotation = rotation;
                leafGO.transform.position = leafPosition;
                leafGO.transform.localScale = new Vector3(leaf.Size, leaf.Size, 0.5f);

                if (environment.Temperature == Enum.Temperature.Max)
                {
                    leafGO.transform.localScale = new Vector3(leaf.Size - 5f, leaf.Size, 0.5f);
                }

                leafGO.name = "Leaf";

                leaf.LeafGO = leafGO;

                if (environment.Minerals == Enum.Minerals.Lack || environment.Temperature == Enum.Temperature.Min)
                {
                    stemEntity.Get<BlockCreateDuration>().Timer = 4f;
                }
                else
                {
                    stemEntity.Get<BlockCreateDuration>().Timer = 2f;
                }
            }
        }

        
        private Vector3 GetLeafPosition(StemComponent stemComponent, float minHeight, float maxHeight)
        {
            while (true)
            {
                bool tooClose = false;
                var leafPosition = stemComponent.stemGO.transform.position + Vector3.up * Random.Range(minHeight, maxHeight);

                foreach (Vector3 existingPosition in _staticData.leafPositions)
                {
                    if (Vector3.Distance(leafPosition, existingPosition) < 0.1f) // 0.1f мин допустимое расстояние между листьями
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                {
                    _staticData.leafPositions.Add(leafPosition);
                    return leafPosition;
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

