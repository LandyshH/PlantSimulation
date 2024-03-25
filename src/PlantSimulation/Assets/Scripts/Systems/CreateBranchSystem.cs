using Assets.Scripts.Services;
using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public sealed class CreateBranchSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld;
        private readonly StaticData _staticData;

        private readonly EcsFilter<StemComponent>.Exclude<BlockCreateDuration> _filter;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var stem = ref _filter.Get1(i);

                if (_staticData.PlantGrowthStage != Enum.PlantGrowthStage.Senile 
                    && _staticData.PlantGrowthStage != Enum.PlantGrowthStage.Embryonic)
                {
                    var branchEntity = _ecsWorld.NewEntity();
                    ref var branch = ref branchEntity.Get<BranchComponent>();

                    branch.Lifetime = 0;
                    branch.Height = 5;
                    branch.Width = 3;

                    branch.Position = new Vector3(stem.Position.x, stem.Position.y, stem.Height - 10);

                    branchEntity.Get<BlockCreateDuration>().Timer = 5f;
                }
            }
        }
    }
}
