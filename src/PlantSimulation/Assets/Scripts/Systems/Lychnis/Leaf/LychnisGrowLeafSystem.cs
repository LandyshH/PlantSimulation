using Leopotam.Ecs;
using UnityEngine;


namespace Assets.Scripts.Systems.Lychnis.Leaf
{
    public class LychnisGrowLeafSystem : IEcsRunSystem
    {
        private EnvironmentSettings environment;
        private StaticData staticData;

        EcsFilter<LeafComponent> _filter;

        public void Run()
        {
            foreach(var i in _filter)
            {
                ref var component = ref _filter.Get1(i);
                var t = staticData.Generations - component.LeafNumber;

                if (t != staticData.CurrGeneration) continue;

                if (component.Width < component.MaxWidth)
                {
                    component.Width += 0.001f * Time.deltaTime;
                }
                if (component.Height < component.MaxHeight)
                {
                    component.Height += component.MaxHeight * 0.1f * Time.deltaTime;
                }

                Vector3 maxScale = new Vector3(component.Width, component.Height, component.Width);
                component.LeafGO.transform.localScale = Vector3.Lerp(component.LeafGO.transform.localScale, maxScale, 2 * Time.deltaTime);
            }
        }
    }
}
