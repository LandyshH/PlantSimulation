using Assets.Scripts.Tags;
using Leopotam.Ecs;
using UnityEngine;
using static UnityEditor.Progress;

public sealed class LeafGrowthAnimationSystem : IEcsRunSystem
{
    private EcsFilter<LeafComponent> _filter;
    private EcsFilter<StemComponent> _stemFilter;
    private EnvironmentSettings environment;
    private readonly StaticData _staticData;

    public void Run()
    {
        if (_staticData.GoToNextStage)
        {
            return;
        }

        ref var stem = ref _stemFilter.Get1(0);
        foreach (var i in _filter)
        {
            ref var leaf = ref _filter.Get1(i);
            ref var entity = ref _filter.GetEntity(i);

            if (entity.Has<SproutTag>())
            {
                Vector3 leafPosition = stem.stemGO.transform.position + Vector3.up * stem.stemGO.transform.localScale.z * 0.25f;
                leaf.LeafGO.transform.position = leafPosition;
            }

            var maxScale = new Vector3(leaf.Width, leaf.Height, 0.5f);

            leaf.LeafGO.transform.localScale = Vector3.Lerp(leaf.LeafGO.transform.localScale, maxScale, 2 * Time.deltaTime);

            if (leaf.Width <= 7f && leaf.Lifetime >= 15)
            {
                MeshRenderer meshRenderer = leaf.LeafGO.GetComponent<MeshRenderer>();
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