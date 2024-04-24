using Leopotam.Ecs;
using System.ComponentModel;
using UnityEngine;

namespace Assets.Scripts.Systems.Lychnis.Leaf
{
    public class MintLeafAnimationSystem : IEcsRunSystem
    {
        private EnvironmentSettings environment;
        private StaticData staticData;
        private MintPrefabs mintPrefabs;

        EcsFilter<LeafComponent> _filter;

        public void Run()
        {
            if (staticData.GoToNextStage) return;
            foreach(var i in _filter)
            {
                ref var component = ref _filter.Get1(i);
                if (component.LeafGO == null) continue;

                if (environment.Temperature == Enum.Temperature.Min && environment.Water == Enum.Water.Excess
                    && !component.ChangedToRust)
                     
                {
                    var t = Random.Range(1, 3);
                    if (t == 2) 
                    {
                        var transform = component.LeafGO.transform;
                        var scale = component.LeafGO.transform.localScale;
                        Object.Destroy(component.LeafGO);

                        var rustLeaf = Object.Instantiate(mintPrefabs.RustLeafPrefab, transform.position, transform.rotation, null);
                        rustLeaf.transform.localScale = scale;

                        component.LeafGO = rustLeaf;
                    }

                    component.ChangedToRust = true; //поставить блок на время вместо флага
                }
                else if (environment.Temperature == Enum.Temperature.Max)
                {
                    var transform = component.LeafGO.transform;
                    var scale = component.LeafGO.transform.localScale;
                    Object.Destroy(component.LeafGO);

                    var dryLeaf = Object.Instantiate(mintPrefabs.DryLeafPrefab, transform.position, transform.rotation, null);
                    dryLeaf.transform.localScale = scale;
                    component.LeafGO = dryLeaf;
                }
            }
        }
    }
}
