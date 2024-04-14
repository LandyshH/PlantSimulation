using UnityEngine;
using Leopotam.Ecs;

public sealed class StemGrowthAnimationSystem : IEcsRunSystem
{
    private EcsFilter<StemComponent> _filter;
    private readonly StaticData _staticData;

    public void Run()
    {
        if (_staticData.PlantGrowthStage == Assets.Scripts.Enum.PlantGrowthStage.Senile)
        {
            return;
        }

        foreach (var i in _filter)
        {
            ref var stem = ref _filter.Get1(i);

            Vector3 maxScale = new Vector3(stem.Width, stem.Width, stem.Height);
             
            stem.stemGO.transform.localScale = Vector3.Lerp(stem.stemGO.transform.localScale, maxScale, 1 * Time.deltaTime);

            if (stem.Width <= 1.4f && _staticData.PlantGrowthStage != Assets.Scripts.Enum.PlantGrowthStage.Embryonic)
            {
                MeshRenderer meshRenderer = stem.stemGO.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    Material newMaterial = new Material(meshRenderer.sharedMaterial)
                    {
                        color = new Color(246f / 255f, 252f / 255f, 94f / 255f)
                    };

                    meshRenderer.material = newMaterial;
                }

            }
        }
    }
}

