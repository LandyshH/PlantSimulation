﻿using Leopotam.Ecs;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public sealed class CreateStemSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld = null;
        private StaticData staticData;

        EcsFilter<RootComponent> _filter;

        public void Init()
        {
            ref var rootComponent = ref _filter.Get1(0);
            Debug.Log("Create stem");

            var stemEntity = _ecsWorld.NewEntity();
            ref var stem = ref stemEntity.Get<StemComponent>();

            //ref var environment = ref _filter.Get2(0);

            stem.Lifetime = 0;
            stem.Position = rootComponent.Position;
            stem.Height = 15;
            stem.Width = 5;
            stem.GrowthStage = Enum.PlantGrowthStage.Juvenile;
        }
    }
}