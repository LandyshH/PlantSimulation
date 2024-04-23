using Assets.Scripts.Enum;
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

                if (i > 0)
                {
                    ref var prevStem = ref _stemFilter.Get1(i - 1);

                    if (prevStem.IsGrowing) 
                    {
                        return; 
                    }
                }
                else
                {
                    staticData.CurrGeneration = 0;
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
                    staticData.CurrGeneration++;
                }
                else 
                {
                    stem.IsGrowing = true;
                } 

                Vector3 maxScale = new Vector3(stem.Width, stem.Height, stem.Width);
                stem.stemGO.transform.localScale = Vector3.Lerp(stem.stemGO.transform.localScale, maxScale, 2 * Time.deltaTime);
            }
        }
    }
}
