using Leopotam.Ecs;
using UnityEngine;

public sealed class FlowerBudGrowthAnimationSystem : IEcsRunSystem
{
    private EcsFilter<FlowerBudComponent> _filter;
    private EcsFilter<StemComponent> _stemFilter;

    private readonly StaticData _staticData;

    public void Run()
    {
        if (_staticData.PlantGrowthStage == Assets.Scripts.Enum.PlantGrowthStage.Senile
            || _staticData.GoToNextStage)
        {
            return;
        }

        ref var stem = ref _stemFilter.Get1(0);

        foreach (var i in _filter)
        {
            ref var bud = ref _filter.Get1(i);

            var position = stem.stemGO.transform.position + Vector3.up * stem.stemGO.transform.localScale.z * 0.25f
                + Vector3.left * 0.04f;
            bud.FlowerBudGO.transform.position = position;
            
            Vector3 maxScale = new Vector3(bud.Size, bud.Size, 3f);
            bud.FlowerBudGO.transform.localScale = Vector3.Lerp(bud.FlowerBudGO.transform.localScale, maxScale,
                1 * Time.deltaTime);

            if (_staticData.PlantGrowthStage == Assets.Scripts.Enum.PlantGrowthStage.Juvenile)
            {
                MeshRenderer meshRenderer = bud.FlowerBudGO.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    Material newMaterial = new Material(meshRenderer.sharedMaterial)
                    {
                        color = new Color(0f, 153f / 255f, 0f)
                    };

                    meshRenderer.material = newMaterial;
                }
            }
            else
            {
                MeshRenderer meshRenderer = bud.FlowerBudGO.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    Material newMaterial = new Material(meshRenderer.sharedMaterial)
                    {
                        color = new Color(102f/255f, 0f, 0f)
                    };

                    meshRenderer.material = newMaterial;
                }
            }
        }
    }
}
    