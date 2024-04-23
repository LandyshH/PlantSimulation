using Leopotam.Ecs;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

namespace Assets.Scripts.Systems.Lychnis.Stem
{
    public class LychnisCreateStemSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld;

        EcsFilter<StemComponent> _filter;

        public LychnisCoronariaPrefabs LychnisCoronariaPrefabs;
        private StaticData staticData;
        private EnvironmentSettings environment;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var stemComponent = ref _filter.Get1(i);
                stemComponent.Lifetime = 0;
                stemComponent.Position = stemComponent.stemGO.transform.position;
                stemComponent.Height = 0;
                stemComponent.MaxHeight = stemComponent.stemGO.transform.localScale.y;
                stemComponent.Width = 0.01f;
                stemComponent.MaxWidth = 0.03f;

                stemComponent.stemGO.transform.localScale = Vector3.zero;
            }
        }
    }
}
////меньше воды -- серебристый цвет листьев. Высота до 90см (40см мин), цветки диаметром до 3м, Требование к освещенности: Солнце. Плод 0.2 см диаметр
// Если поливать такой цветок чересчур обильно либо очень часто, то это может стать причиной развития ржавчины, корневой гнили и пятнистости листвы.