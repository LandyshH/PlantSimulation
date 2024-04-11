using Assets.Scripts.Tags;
using Leopotam.Ecs;
using UnityEngine;
using static UnityEditor.Progress;

public class LeafGrowthAnimationSystem : IEcsRunSystem
{
    private EcsFilter<LeafComponent> _filter;
    private EcsFilter<StemComponent> _stemFilter;
    private readonly StaticData _staticData;

    public void Run()
    {
        if (_staticData.PlantGrowthStage == Assets.Scripts.Enum.PlantGrowthStage.Senile 
            || _staticData.PlantGrowthStage == Assets.Scripts.Enum.PlantGrowthStage.Embryonic
            || _staticData.GoToNextStage)
        {
            return;
        }

        ref var stem = ref _stemFilter.Get1(0);
        foreach (var i in _filter)
        {
            ref var leaf = ref _filter.Get1(i);
            ref var entity = ref _filter.GetEntity(i);
            if (entity.Has<SproutTag>())
            {
                leaf.LeafGO.transform.position += Vector3.up * _staticData.StemHeightDiff;
            }

            Vector3 maxScale = new Vector3(leaf.Size, leaf.Size, 0.5f);
            leaf.LeafGO.transform.localScale = Vector3.Lerp(leaf.LeafGO.transform.localScale, maxScale, 1 * Time.deltaTime);
        }
    }
}