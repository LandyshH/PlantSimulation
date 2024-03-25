using Assets.Scripts.Components.Events;
using Assets.Scripts.Systems;
using Leopotam.Ecs;
using UnityEngine;
using Voody.UniLeo;

public class EcsGamestartup : MonoBehaviour
{
    public StaticData configuration;
    public EnvironmentSettings environmentSettings;
    public UI ui;
    private EcsWorld ecsWorld;
    private EcsSystems systems;

    private void Start()
    {
        ecsWorld = new EcsWorld();
        systems = new EcsSystems(ecsWorld);

        systems.ConvertScene();

        AddInjections();
        AddOneFrames();
        AddSystems();

        systems.Init();
    }

    private void AddInjections()
    {
        systems
            .Inject(configuration)
            .Inject(environmentSettings)
            .Inject(ui)
            ;
    }

    private void AddSystems()
    {
        systems
            .Add(new GoToNextStageSendEventSystem())
            .Add(new GoToNextStageSystem())
            .Add(new InputSystem())

            .Add(new CreateSeedSystem())
            .Add(new SeedGrowthSystem())

            .Add(new CreateRootSystem())
            .Add(new RootGrowthSystem())

            .Add(new CreateStemSystem())
            .Add(new StemGrowthSystem())

            .Add(new CreateBranchSystem())
            .Add(new BranchGrowthSystem())


            .Add(new CreateLeafAndFlowerSystem())
            .Add(new LeafGrowthSystem())
            .Add(new FlowerGrowthSystem())
            ;
    }

    private void AddOneFrames()
    {
        systems.OneFrame<NextStageGrowEvent>();
    }

    private void Update()
    {
        systems.Run();
    }

    private void OnDestroy()
    {
        if ( systems == null ) 
        {
            return; 
        }

        systems.Destroy();
        systems = null;
        ecsWorld.Destroy();
        ecsWorld = null;
    }
}
