using Assets.Scripts.Components.Events;
using Assets.Scripts.ProceduralGeneration.Sunflower.ScriptableObjects;
using Assets.Scripts.Systems.Lychnis.Flower;
using Assets.Scripts.Systems.Lychnis.Leaf;
using Assets.Scripts.Systems.Lychnis.Stem;
using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Voody.UniLeo;

//Горицвет (ЛИхнис)
namespace Assets.Scripts.Systems.Lychnis
{
    public class LychnisEcsGameStartup : MonoBehaviour
    {
        public StaticData configuration;
        public EnvironmentSettings environmentSettings;
        public UI ui;
        public MintPrefabs LychnisCoronariaPrefabs;
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
                .Inject(LychnisCoronariaPrefabs)
                ;
        }

        private void AddSystems()
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

                .Add(new LychnisCreateStemSystem())
                .Add(new LychnisGrowStemSystem())
                .Add(new LychnisAnimationStemSystem())

                .Add(new LychnisCreateLeafSystem())
                .Add(new LychnisGrowLeafSystem())
                .Add(new MintLeafAnimationSystem())

                .Add(new LychnisCreateFlowerSystem())
                .Add(new LychnisGrowFlowerSystem())
                .Add(new MintFlowerAnimationSystem())
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
}

