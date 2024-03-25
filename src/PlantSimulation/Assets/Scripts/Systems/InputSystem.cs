using Assets.Scripts.Components.Events;
using Assets.Scripts.Tags;
using Leopotam.Ecs;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class InputSystem : IEcsRunSystem
    {
        private readonly UI ui;
        private readonly StaticData staticData;
        private readonly EcsFilter<EnvironmentWindowTag, NextStageGrowEvent> _filter;

        public void Run()
        {
            foreach (var i in _filter)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ref var entity = ref _filter.GetEntity(i);
                    staticData.GoToNextStage = false;
                    ui.environmentWindowScreen.SetActive(false);

                    //if(staticData.PlantGrowthStage == Enum.PlantGrowthStage.Embryonic)
                    //{
                    staticData.PlantGrowthStage += 1;
                    //}
                    //staticData.PlantGrowthStage += 1;

                    Time.timeScale = 1f;
                    //entity.Del<NextStageGrowEvent>();
                }
            }
        }
    }
}
