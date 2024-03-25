using Assets.Scripts.Components.Events;
using Assets.Scripts.Tags;
using Leopotam.Ecs;
namespace Assets.Scripts.Systems
{
    public class GoToNextStageSendEventSystem : IEcsRunSystem
    {
        private readonly StaticData _staticData;
        private readonly EcsFilter<EnvironmentWindowTag> _filter;

        public void Run()
        {
            if (!_staticData.GoToNextStage)
            {
                return;
            }

            foreach(var i in _filter)
            {
                ref var entity = ref _filter.GetEntity(i);
                entity.Get<NextStageGrowEvent>();
            }
        }
    }
}
