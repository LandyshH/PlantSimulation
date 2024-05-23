using Leopotam.Ecs;
using UnityEngine;

public sealed class MintStemAnimationSystem : IEcsRunSystem
{
    private EcsFilter<StemComponent> _stemFilter;
    private EcsFilter<LeafComponent> _leafFilter;
    private EcsFilter<FlowerComponent> _flowerFilter;
    private readonly StaticData _staticData;

    public void Run()
    {
        if (_staticData.GoToNextStage)
        {
            return;
        }

        foreach (var i in _stemFilter)
        {
            ref var stem = ref _stemFilter.Get1(i);
            Vector3 maxScale = new Vector3(stem.Width, stem.MaxHeight, stem.Width);

            stem.stemGO.transform.localScale = Vector3.Lerp(stem.stemGO.transform.localScale, maxScale,
                0.8f * Time.deltaTime);
        }

        foreach (var j in _leafFilter)
        {
            ref var leaf = ref _leafFilter.Get1(j);

            Vector3 targetPosition = leaf.TargetPosition;
            leaf.LeafGO.transform.position = Vector3.Lerp(leaf.LeafGO.transform.position, targetPosition, 0.1f * Time.deltaTime);

            Vector3 leafMaxScale = new Vector3(leaf.MaxWidth, 1f, leaf.MaxHeight);
            leaf.LeafGO.transform.localScale = Vector3.Lerp(leaf.LeafGO.transform.localScale, leafMaxScale,
                0.3f * Time.deltaTime);
        }

        foreach (var k in _flowerFilter)
        {
            ref var flower = ref _flowerFilter.Get1(k);

            Vector3 targetPosition = flower.TargetPosition;
            flower.FlowerGO.transform.position = Vector3.Lerp(flower.FlowerGO.transform.position, targetPosition,
                0.1f * Time.deltaTime);

            Vector3 flowerMaxScale = new Vector3(flower.maxSize, flower.maxSize, flower.maxSize);
            flower.FlowerGO.transform.localScale = Vector3.Lerp(flower.FlowerGO.transform.localScale, flowerMaxScale,
                0.1f * Time.deltaTime);
        }
    }
}
