using Assets.Scripts.ProceduralGeneration.Sunflower.ScriptableObjects;
using Leopotam.Ecs;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

namespace Assets.Scripts.Systems.Lychnis.Stem
{
    public class LychnisCreateStemSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld;

        EcsFilter<RootComponent> _filter;

        public SunflowerObjects sunflowerObjects;
        private StaticData staticData;
        private EnvironmentSettings environment;

        public void Init()
        {
            ref var rootComponent = ref _filter.Get1(0);

            var stemEntity = _ecsWorld.NewEntity();
            ref var stem = ref stemEntity.Get<StemComponent>();

            stem.Lifetime = 0;
            stem.Position = rootComponent.Position;
            stem.Height = 0;
            stem.MaxHeight = 15f;
            stem.Width = 0.1f;
            stem.MaxWidth = 3f;
            staticData.StemHeightDiff = 0;

            var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();

            var stemGO = Object.Instantiate(sunflowerObjects.StemPrefab, plant.transform);
            stemGO.name = "Stem";
            stemGO.transform.localScale = new Vector3(stem.Width, stem.Width, stem.Height);
            stemGO.transform.localPosition = Vector3.zero;
        }
    }
}
////меньше воды -- серебристый цвет листьев. Высота до 90см (40см мин), цветки диаметром до 3м, Требование к освещенности: Солнце. Плод 0.2 см диаметр
// Если поливать такой цветок чересчур обильно либо очень часто, то это может стать причиной развития ржавчины, корневой гнили и пятнистости листвы.