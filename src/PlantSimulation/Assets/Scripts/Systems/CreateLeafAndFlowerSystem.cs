using Leopotam.Ecs;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public sealed class CreateLeafAndFlowerSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld;
        private EnvironmentSettings environment;
        private readonly StaticData _staticData;

        EcsFilter<StemComponent> _filter;
        EcsFilter<BranchComponent> _branchFilter;

        //добавить таймер
        public void Run()
        {
            foreach(var i in _filter)
            {
                ref var stem = ref _filter.Get1(0);
               // ref var environment = ref _filter.Get2(i);

                if (_staticData.PlantGrowthStage == Enum.PlantGrowthStage.Senile || _staticData.PlantGrowthStage == Enum.PlantGrowthStage.Embryonic)
                {
                    continue;
                }

               // Debug.Log("Spawn flower or leaf");

                foreach (var j in _branchFilter)
                {
                    ref var branch = ref _branchFilter.Get1(j);

                    if (branch.HasLeafOrFlower)
                    {
                        continue;
                    }

                    var rnd = new System.Random();
                    var random = rnd.Next(1, 3);
                    // stem height
                    var position = new Vector3(stem.Position.x, stem.Position.y, branch.Position.z);

                    if (environment.Temperature == Enum.Temperature.Min && random == 1)
                    {
                        Debug.Log("Temperature min: no leaf or flower");
                        continue;
                    }

                    if (_staticData.PlantGrowthStage == Enum.PlantGrowthStage.Juvenile)
                    {
                        var leafEntity = CreateLeaf(position);
                        continue;
                    }

                    if (environment.Minerals == Enum.Minerals.Optimal)
                    {
                        if (random == 1 || random == 2)
                        {
                            CreateLeaf(position);
                        }
                        else
                        {
                            CreateFlower(position);
                        }
                    }
                    else
                    {
                        if (random == 1 || random == 2)
                        {
                            CreateFlower(position);
                        }
                        else
                        {
                            CreateLeaf(position);
                        }
                    }
                }
            }
        }

        private EcsEntity CreateLeaf(Vector3 position)
        {
            var leafEntity = _ecsWorld.NewEntity();
            ref var leaf = ref leafEntity.Get<LeafComponent>();

            leaf.Lifetime = 0;
            leaf.Size = 10;

            leaf.Position = position;

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
