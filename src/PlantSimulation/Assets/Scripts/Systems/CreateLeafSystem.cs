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
                ref var stem = ref _stemFilter.Get1(i);

                //var rnd = new System.Random();
                //var random = rnd.Next(1, 3);
                var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();
                var leafEntity = CreateLeaf(stem, plant.transform);

                // spawn in 2 sec
                stemEntity.Get<BlockCreateDuration>().Timer = 2f;
            }
        }

        private EcsEntity CreateLeaf(StemComponent stemComponent, Transform parentTransform)
        {
            var leafEntity = _ecsWorld.NewEntity();
            ref var leaf = ref leafEntity.Get<LeafComponent>();
            leaf.Lifetime = 0;
            leaf.Size = 10;

            var angle = Random.Range(0f, 360f);
            var rotation = Quaternion.Euler(-60f, angle, 0f);

            var minHeight = stemComponent.stemGO.transform.localScale.z * 0.05f;
            var maxHeight = stemComponent.stemGO.transform.localScale.z * 0.25f;
            //Debug.Log(minHeight.ToString() + " " + maxHeight.ToString());

            var leafPosition = stemComponent.stemGO.transform.position + Vector3.up * Random.Range(minHeight, maxHeight);

            var leafGO = Object.Instantiate(_sunflowerObjects.LeafPrefab, parentTransform);
            leafGO.transform.localRotation = rotation;
            leafGO.transform.position = leafPosition;
            leafGO.transform.localScale = new Vector3(leaf.Size, leaf.Size, 0.5f);

            leaf.LeafGO = leafGO;

            return leafEntity;
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

/*
 public void CreateLeafs(GameObject stem, GameObject leafPrefab)
    {
        List<Vector3> leafPositions = new List<Vector3>();

        for (int i = 0; i < leafCount; i++)
        {
            float angle = Random.Range(0f, 360f);
            Quaternion rotation = Quaternion.Euler(-60f, angle, 0f);

            float minHeight = stemHeight * 0.05f;
            float maxHeight = stemHeight * 0.25f;

            Debug.Log(minHeight.ToString() + " " + maxHeight.ToString());

            Vector3 leafPosition = stem.transform.position + Vector3.up * Random.Range(minHeight, maxHeight);

            bool tooClose = false;
            foreach (Vector3 existingPosition in leafPositions)
            {
                if (Vector3.Distance(leafPosition, existingPosition) < 0.1f) // 0.2f мин допустимое расстояние между листьями
                {
                    tooClose = true;
                    break;
                }
            }

            if (tooClose)
            {
                continue;
            }

            leafPositions.Add(leafPosition);

            GameObject leaf = Instantiate(leafPrefab, sunflower.transform);
            leaf.transform.localRotation = rotation;
            leaf.transform.position = leafPosition;
            leaf.transform.localScale = new Vector3(leafWidth, leafHeight, 0.5f);
        }
    }
 */