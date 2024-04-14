using Leopotam.Ecs;
using UnityEngine;

public sealed class PetalAnimationSystem : IEcsRunSystem
{
    private readonly StaticData _staticData;
    private EcsFilter<PetalComponent> _filter;
    private readonly EcsFilter<FlowerBudComponent> _budFilter;

    public void Run()
    {
        if (_staticData.GoToNextStage 
            || _staticData.PlantGrowthStage != Assets.Scripts.Enum.PlantGrowthStage.MaturityAndReproduction)
        {
            return;
        }

        ref var bud = ref _budFilter.Get1(0);

        foreach (var i in _filter)
        {
            ref var petal = ref _filter.Get1(i);

            Vector3 maxScale = new Vector3(petal.Size, petal.Size, petal.Size);
            petal.PetalGO.transform.localScale = Vector3.Lerp(petal.PetalGO.transform.localScale, maxScale, 1 * Time.deltaTime);
        }
    }
}
