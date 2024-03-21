using Leopotam.Ecs;

namespace Assets.Scripts.Systems
{
    public class SeedAnimationSystem : IEcsRunSystem
    {
        private EcsFilter<SeedComponent> _filter = null;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var seedComponent = ref _filter.Get1(i);

                
            }
        }
    }
}
