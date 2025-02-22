﻿using Assets.Scripts.Enum;
using Assets.Scripts.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Assets.Scripts.Systems.Lychnis.Stem
{
    public class LychnisGrowStemSystem : IEcsRunSystem
    {
        private EnvironmentSettings environment;
        private StaticData staticData;

        EcsFilter<StemComponent> _stemFilter;

        public void Run()
        {
            foreach (var i in _stemFilter)
            {
                ref var stem = ref _stemFilter.Get1(i);
                stem.Lifetime += Time.deltaTime;
                if (stem.Lifetime < Time.deltaTime * i)
                {
                    continue;
                }

                if (stem.Width < stem.MaxWidth)
                {
                    stem.Width += 0.001f * Time.deltaTime;
                }

                if (stem.Height < stem.MaxHeight)
                {
                    stem.Height += stem.MaxHeight * 0.1f * Time.deltaTime;
                }

                if (stem.Height >= stem.MaxHeight)
                {
                    stem.IsGrowing = false;
                }

                Vector3 maxScale = new Vector3(stem.Width, stem.Height, stem.Width);
                stem.stemGO.transform.localScale = Vector3.Lerp(stem.stemGO.transform.localScale, maxScale, 2 * Time.deltaTime);
            }
        }
    }
}
