using Leopotam.Ecs;
using UnityEngine;

public sealed class StrawberryStemAnimationSystem : IEcsRunSystem
{
    private EcsFilter<StemComponent> _filter;
    private readonly StaticData _staticData;

    public void Run()
    {
        if (_staticData.GoToNextStage)
        {
            return;
        }

        foreach (var i in _filter)
        {
            ref var stem = ref _filter.Get1(i);

            Vector3 maxScale = new Vector3(stem.Width, stem.MaxHeight, stem.Width);

            stem.stemGO.transform.localScale = Vector3.Lerp(stem.stemGO.transform.localScale, maxScale,
                0.3f * Time.deltaTime);
        }
    }
}

