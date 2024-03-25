using Assets.Scripts.Components.Events;
using Assets.Scripts.Tags;
using Leopotam.Ecs;
using UnityEngine;

public class GoToNextStageSystem : IEcsRunSystem
{
    private readonly UI ui;
    private readonly EcsFilter<EnvironmentWindowTag, NextStageGrowEvent> _filter;

    public void Run()
    {
        foreach (var i in _filter)
        {
            ref var entity = ref _filter.GetEntity(i);
            Time.timeScale = 0f;
            ui.environmentWindowScreen.SetActive(true);
        }
    }
}
