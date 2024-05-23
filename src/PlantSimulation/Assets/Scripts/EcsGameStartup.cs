using Assets.Scripts.Components.Events;
using Assets.Scripts.Enum;
using Assets.Scripts.LSystem;
using Assets.Scripts.LSystem.Strawberry;
using Assets.Scripts.ProceduralGeneration.Sunflower.ScriptableObjects;
using Assets.Scripts.Systems;
using Assets.Scripts.Systems.Lychnis;
using Assets.Scripts.Systems.Lychnis.Flower;
using Assets.Scripts.Systems.Lychnis.Leaf;
using Leopotam.Ecs;
using UnityEngine;
using Voody.UniLeo;

public class EcsGamestartup : MonoBehaviour
{
    public StaticData configuration;
    public EnvironmentSettings environmentSettings;
    public UI ui;
    public PlantType plantType;
    public SunflowerObjects sunflowerObjects;
    public MintPrefabs MintPrefabs;
    public StrawberryObjects StrawberryObjects;
    private EcsWorld ecsWorld;
    private EcsSystems systems;

    private void Start()
    {
        ecsWorld = new EcsWorld();
        systems = new EcsSystems(ecsWorld);

        systems.ConvertScene();

        AddInjections();
        AddOneFrames();

        switch (plantType)
        {
            case PlantType.Sunflower:
                AddSunflowerSystems();
                break;
            case PlantType.Mint:
                AddMintSystems();
                break;
            case PlantType.Strawberry:
                AddStrawberrySystems();
                break;
        }

        systems.Init();
    }

    private void AddInjections()
    {
        systems
            .Inject(configuration)
            .Inject(environmentSettings)
            .Inject(ui)
            .Inject(plantType)
            .Inject(sunflowerObjects)
            .Inject(MintPrefabs)
            .Inject(StrawberryObjects)
            ;
    }

    private void AddSunflowerSystems()
    {
        systems
            .Add(new CreateBlockSystem())
            .Add(new GoToNextStageSendEventSystem())
            .Add(new GoToNextStageSystem())
            .Add(new InputSystem())

            .Add(new CreateSeedSystem())
            .Add(new SeedGrowthSystem())

            .Add(new CreateRootSystem())
            .Add(new RootGrowthSystem())

            .Add(new CreateStemSystem())
            .Add(new StemGrowthSystem())
            .Add(new StemGrowthAnimationSystem())

            .Add(new CreateBranchSystem())
            .Add(new BranchGrowthSystem())


            .Add(new CreateLeafSystem())
            .Add(new LeafGrowthSystem())
            .Add(new FlowerGrowthSystem())
            .Add(new LeafGrowthAnimationSystem())

            .Add(new CreateFlowerBudSystem())
            .Add(new FlowerBudGrowthSystem())
            .Add(new FlowerBudGrowthAnimationSystem())

            .Add(new CreatePetalsSystem())
            .Add(new PetalGrowthSystem())
            .Add(new PetalAnimationSystem())
            ;
    }

    private void AddMintSystems()
    {
        systems
                .Add(new FirstInitSystem())
                .Add(new MintGenerationSystem())
                .Add(new MintInputSystem())
                .Add(new MintFlowerAnimationSystem())
                .Add(new MintLeafAnimationSystem())
                .Add(new MintStemAnimationSystem())
               // .Add(new LychnisGrowStemSystem())
               // .Add(new LychnisGrowLeafSystem())
                /*.Add(new CreateBlockSystem())
                .Add(new GoToNextStageSendEventSystem())
                .Add(new GoToNextStageSystem())
                .Add(new InputSystem())

                .Add(new CreateRootSystem())
                .Add(new RootGrowthSystem())

                .Add(new LychnisGrowStemSystem())*/
                //.Add(new LychnisGrowLeafSystem())

                //.Add(new LychnisAnimationStemSystem())
                /*.Add(new CreateBlockSystem())
                .Add(new GoToNextStageSendEventSystem())
                .Add(new GoToNextStageSystem())
                .Add(new InputSystem())

                .Add(new CreateSeedSystem())
                .Add(new SeedGrowthSystem())

                .Add(new CreateRootSystem())
                .Add(new RootGrowthSystem())

                //.Add(new LychnisCreateStemSystem())
                .Add(new LychnisGrowStemSystem())
                .Add(new LychnisAnimationStemSystem())

                .Add(new LychnisCreateLeafSystem())
                .Add(new LychnisGrowLeafSystem())
                .Add(new LychnisLeafAnimationSystem())

                .Add(new LychnisCreateFlowerSystem())
                .Add(new LychnisGrowFlowerSystem())
                .Add(new LychnisFlowerAnimationSystem())*/
                ;
    }

    private void AddStrawberrySystems()
    {
        systems
                .Add(new FirstInitSystem())
                .Add(new StrawberryGenerationSystem())
                .Add(new MintInputSystem())
                .Add(new StrawberryStemAnimationSystem())
                //.Add(new StrawberryLeafAnimationSystem())
                .Add(new StrawberryAnimationSystem())
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
        if (systems == null)
        {
            return;
        }

        systems.Destroy();
        systems = null;
        ecsWorld.Destroy();
        ecsWorld = null;
    }
}
