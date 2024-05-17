using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public sealed class StrawberryLeafAnimationSystem : IEcsRunSystem
{
    private EcsFilter<LeafComponent> _filter;
    private readonly StaticData _staticData;

    public void Run()
    {
        if (_staticData.GoToNextStage)
        {
            return;
        }

        foreach (var i in _filter)
        {
            ref var leaf = ref _filter.Get1(i);
            Vector3 maxScale = new Vector3(leaf.MaxHeight, leaf.MaxHeight, leaf.MaxHeight);

            leaf.LeafGO.transform.localScale = Vector3.Lerp(leaf.LeafGO.transform.localScale, maxScale,
                0.3f * Time.deltaTime);
        }
    }
}
