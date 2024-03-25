using Leopotam.Ecs;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public sealed class CreateLeafAndFlowerSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld;
        private EnvironmentSettings environment;
        private readonly StaticData _staticData;

        private readonly EcsFilter<BranchComponent>
            .Exclude<BlockCreateDuration> _filter;

        public void Run()
        {

            if (_staticData.PlantGrowthStage == Enum.PlantGrowthStage.Senile
            || _staticData.PlantGrowthStage == Enum.PlantGrowthStage.Embryonic
            || _staticData.GoToNextStage)
            {
                return;
            }


            foreach (var i in _filter)
            {
                ref var branchEntity = ref _filter.GetEntity(i);
                ref var branch = ref _filter.Get1(i);

                if (branch.HasLeafOrFlower)
                {
                    return;
                }

                var rnd = new System.Random();
                var random = rnd.Next(1, 3);
                // stem height
                var position = new Vector3(0, 0, branch.Position.z);

                if (environment.Temperature == Enum.Temperature.Min && random == 1)
                {
                    Debug.Log("Temperature min: no leaf or flower");
                    return;
                }

                if (_staticData.PlantGrowthStage == Enum.PlantGrowthStage.Juvenile)
                {
                    CreateLeaf(position);
                    return;
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

                branch.HasLeafOrFlower = true;

                // spawn in 5 sec
                branchEntity.Get<BlockCreateDuration>().Timer = 5f;

                Debug.Log("Spawn flower or leaf");
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
