using System.Collections;
using UnityEngine;
using Leopotam.Ecs;

public class StemGrowthAnimationSystem : IEcsRunSystem
{
    private EcsFilter<StemComponent> _filter;

    public void Run()
    {
        foreach (var i in _filter)
        {
            ref var stem = ref _filter.Get1(i);

            Vector3 maxScale = new Vector3(stem.Width, stem.Width, stem.Height);
             
            stem.stemGO.transform.localScale = Vector3.Lerp(stem.stemGO.transform.localScale, maxScale, 1 * Time.deltaTime);
        }
    }
}
