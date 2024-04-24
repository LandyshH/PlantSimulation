using Assets.Scripts.Components.Events;
using Assets.Scripts.Tags;
using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Systems.Lychnis
{
    public class MintInputSystem : IEcsRunSystem
    {
        private readonly UI ui;
        private readonly StaticData staticData;
        private readonly EcsFilter<EnvironmentWindowTag, NextStageGrowEvent> _filter;

        public void Run()
        {
            //foreach (var i in _filter)
            //{
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (staticData.PlantGrowthStage == Enum.PlantGrowthStage.Senile)
                {
                    return;
                }

                    staticData.GoToNextStage = !staticData.GoToNextStage;

                    ui.environmentWindowScreen.SetActive(staticData.GoToNextStage);

                    if (!staticData.GoToNextStage)
                        staticData.PlantGrowthStage += 1;

                    Time.timeScale = staticData.GoToNextStage ? 0f : 1f;
                }
            //}
        }
    }
}
