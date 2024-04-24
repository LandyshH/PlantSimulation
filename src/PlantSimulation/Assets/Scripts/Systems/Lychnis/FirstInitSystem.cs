using Leopotam.Ecs;


namespace Assets.Scripts.Systems.Lychnis
{
    public class FirstInitSystem : IEcsInitSystem
    {
        private EnvironmentSettings environment;
        private StaticData staticData;
        private readonly UI ui;

        public void Init()
        {
            staticData.PlantGrowthStage = Enum.PlantGrowthStage.Embryonic;
            ui.environmentWindowScreen.SetActive(false);
            staticData.JuvnileGenerated = false;
            staticData.SproutGenerated = false;
            staticData.MaturityGenerated = false;
        }
    }
}
