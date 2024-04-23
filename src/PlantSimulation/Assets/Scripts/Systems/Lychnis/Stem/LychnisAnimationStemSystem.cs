using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Systems.Lychnis.Stem
{
    public class LychnisAnimationStemSystem : IEcsRunSystem
    {
        private EcsFilter<StemComponent> _filter;
        private readonly StaticData _staticData;

        public void Run()
        {

            foreach (var i in _filter)
            {
                ref var stem = ref _filter.Get1(i);

                Vector3 maxScale = new Vector3(stem.Width, stem.Height, stem.Width);

                stem.stemGO.transform.localScale = Vector3.Lerp(stem.stemGO.transform.localScale, maxScale, 2 * Time.deltaTime);
            }
        }
    }
}
