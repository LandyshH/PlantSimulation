using Assets.Scripts;
using Assets.Scripts.Systems;
using Leopotam.Ecs;
using UnityEngine;
using Voody.UniLeo;

public class EcsGamestartup : MonoBehaviour
{
    public StaticData _configuration;
    public EnvironmentSettings _environmentSettings;
    private EcsWorld _ecsWorld;
    private EcsSystems _ecsSystems;

    private void Start()
    {
        _ecsWorld = new EcsWorld();
        _ecsSystems = new EcsSystems(_ecsWorld);

        _ecsSystems.ConvertScene();

        AddInjections();
        AddOneFrames();
        AddSystems();

        _ecsSystems.Init();
    }

    private void AddInjections()
    {
        _ecsSystems
            .Inject(_configuration)
            .Inject(_environmentSettings);
    }

    private void AddSystems()
    {
        _ecsSystems
            .Add(new CreateSeedSystem())
            .Add(new SeedGrowthSystem())
           // .Add(new SeedAnimationSystem())
            .Add(new CreateRootSystem())
            .Add(new RootGrowthSystem())

            //.Add(new CreateStemSystem())
            .Add(new StemGrowthSystem())

            .Add(new CreateBranchSystem())
            .Add(new BranchGrowthSystem())


            .Add(new CreateLeafAndFlowerSystem())
            .Add(new LeafGrowthSystem())
            .Add(new FlowerGrowthSystem());
    }

    private void AddOneFrames()
    {

    }

    private void Update()
    {
        _ecsSystems.Run();
    }

    private void OnDestroy()
    {
        if ( _ecsSystems == null ) 
        {
            return; 
        }

        _ecsSystems.Destroy();
        _ecsSystems = null;
        _ecsWorld.Destroy();
        _ecsWorld = null;
    }
}
