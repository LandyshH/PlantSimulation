using System.Collections;
using UnityEngine;
using Leopotam.Ecs;

public class StemGrowthAnimationSystem : IEcsRunSystem
{
    private EcsFilter<StemComponent> filter;

    private readonly StaticData _staticData;

    public void Run()
    {
        foreach (var i in filter)
        {
            if(_staticData.GoToNextStage)
            {
                return;
            }

            ref var stem = ref filter.Get1(i);

            Vector3 maxScale = new Vector3(stem.Width, stem.Width, stem.Height);
             
            stem.stemGO.transform.localScale = Vector3.Lerp(stem.stemGO.transform.localScale, maxScale, 1 * Time.deltaTime);
        }
    }

    /*private IEnumerator Grow()
    {
        Vector3 startScale = transform.localScale;
        Vector3 maxScale = new Vector3(maxSize, maxSize, maxSize);
        do
        {
            transform.localScale = Vector3.Lerp(startScale, maxScale, timer / growtime);
            timer += Time.deltaTime;
            yield return null;
        }
        while (!_staticData.GoToNextStage);
    }*/
}
