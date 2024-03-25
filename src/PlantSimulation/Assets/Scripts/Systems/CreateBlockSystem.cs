using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    sealed class CreateBlockSystem : IEcsRunSystem
    {
        private readonly EcsFilter<BlockCreateDuration> _filter;
        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var entity = ref _filter.GetEntity(i);
                ref var block = ref _filter.Get1(i);
                block.Timer -= Time.deltaTime;

                if (block.Timer <= 0)
                {
                    entity.Del<BlockCreateDuration>();
                }
            }
        }
    }
}
