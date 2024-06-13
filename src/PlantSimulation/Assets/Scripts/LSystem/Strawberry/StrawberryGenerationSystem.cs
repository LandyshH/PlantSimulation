using Leopotam.Ecs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.LSystem.Strawberry
{
    public class StrawberryGenerationSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld;

        private StaticData staticData;
        private EnvironmentSettings environment;
        public StrawberryObjects StrawberryPrefabs;

        private float StemLength = 0.6f;
        private float StemWidth = 1f;
        private float flowerSize = 1f;
        private float LeafLength = 0.6f;
        private float LeafWidth = 0.6f;

        private float angle = 60;
        private float leafAngle = 60;

        private string axiom = "[F[-L][L][+L]]";

        private Stack<Turtle> stack = new Stack<Turtle>();

        public void Run()
        {

            if (staticData.PlantGrowthStage == Enum.PlantGrowthStage.Embryonic && !staticData.SproutGenerated)
            {
                DrawLSystem(axiom);

                staticData.SproutGenerated = true;
                return;
            }

            if (staticData.PlantGrowthStage == Enum.PlantGrowthStage.Juvenile && !staticData.JuvenileGenerated)
            {
                CalculateGrowth();

                GenerateLeafStemString();

                Debug.Log(axiom);
                DrawLSystem(axiom);

                staticData.JuvenileGenerated = true;
                return;
            }

            if (staticData.PlantGrowthStage == Enum.PlantGrowthStage.MaturityAndReproduction 
                && !staticData.MaturityGenerated)
            {
                CalculateGrowth();

                GenerateStrawberryStemString();

                DrawLSystem(axiom);

                Debug.Log(axiom);
                staticData.MaturityGenerated = true;
                return;
            }

            if (staticData.MaturityGenerated)
            {
                return;
            }
        }

        private void CalculateGrowth()
        {
            /*private float StemLength = 0.6f;
        private float StemWidth = 1f;
        private float flowerSize = 1f;
        private float LeafLength = 0.6f;*/

            if (environment.Water == Enum.Water.Lack || environment.Water == Enum.Water.Excess)
            {
                StemLength -= 0.003f;

                flowerSize -= 0.1f;

                LeafWidth -= 0.07f;
                LeafLength -= 0.07f;
            }

            if (environment.Temperature == Enum.Temperature.Max)
            {
                StemLength -= 0.3f;

                flowerSize -= 0.2f;

                LeafWidth -= 0.2f;
                LeafLength -= 0.2f;
            }

            if (environment.Temperature == Enum.Temperature.Min)
            {
                StemLength -= 0.005f;
                flowerSize -= 0.1f;
                LeafWidth -= 0.1f;
            }

            if (environment.Oxygen == Enum.Oxygen.Lack || environment.Oxygen == Enum.Oxygen.Excess)
            {
                StemLength -= 0.002f;
                LeafWidth -= 0.05f;
                LeafLength -= 0.05f;
            }

            if (environment.Light == Enum.LightColor.Darkness)
            {
                StemLength -= 0.0005f;

                LeafWidth -= 0.1f;
                LeafLength -= 0.1f;
            }
        }

        private void GenerateStrawberryStemString()
        {
            string output = "[F";
            var strwStemCount = Random.Range(2, 4);

            if (environment.Temperature == Enum.Temperature.Max)
            {
                strwStemCount = 1;
            }

            for (var i = 0; i < strwStemCount; i++)
            {
                output += "[S]";
            }

            output += "]";

            axiom = output;
        }

        private void GenerateLeafStemString()
        {
            string output = "";
            var leafStemCount = Random.Range(2, 4);
            for (var i = 0; i < leafStemCount; i++)
            {
                output += "[F[-L][L][+L]]";
            }

            axiom = output;
        }

        private void DrawLSystem(string lSystemString)
        {
            var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();

            var turtle = new Turtle(plant.transform.rotation, plant.transform.position, Vector3.up * StemLength);

            for (int i = 0; i < lSystemString.Length; i++)
            {
                char c = lSystemString[i];

                switch (c)
                {
                    case 'F':
                        StemLength = Random.Range(0.4f, 1.5f);

                        if (environment.Temperature == Enum.Temperature.Max)
                        {
                            StemLength = Random.Range(0.4f, 0.7f);
                        }

                        if (staticData.PlantGrowthStage == Enum.PlantGrowthStage.Embryonic)
                            StemLength = 0.6f;

                        if (staticData.PlantGrowthStage == Enum.PlantGrowthStage.MaturityAndReproduction)
                            StemLength = Random.Range(1f, 1.5f);

                        turtle.step = Vector3.up * StemLength;

                        if (staticData.PlantGrowthStage != Enum.PlantGrowthStage.Embryonic)
                        {
                            if (staticData.PlantGrowthStage == Enum.PlantGrowthStage.MaturityAndReproduction)
                            {
                                turtle.RotateX(Random.Range(-20, 20));
                                turtle.RotateZ(Random.Range(-20, 20));
                                turtle.RotateY(Random.Range(-15, 15));
                            }
                            else
                            {
                                turtle.RotateX(Random.Range(-45, 45));
                                turtle.RotateZ(Random.Range(-20, 20));
                                turtle.RotateY(Random.Range(-15, 15));
                            }
                        }

                        DrawStem(turtle.position, turtle.direction);
                        turtle.Forward();
                        turtle.Forward();

                        break;
                    case 'L':
                        DrawLeaf(turtle.position, turtle.direction);
                        break;
                    case 'S':
                        turtle.RotateY(Random.Range(-270, 270));
                        turtle.RotateZ(Random.Range(-20, 10));
                        DrawFlower(turtle.position, turtle.direction);
                        break;
                    case '+':
                        turtle.RotateX(angle);
                        break;
                    case '-':
                        turtle.RotateX(-angle);
                        break;
                    case '[':
                        stack.Push(turtle);
                        break;
                    case ']':
                        turtle = stack.Pop();
                        break;
                }
            }
        }

        private void DrawStem(Vector3 position, Quaternion direction)
        {
            var stem = GameObject.Instantiate(StrawberryPrefabs.StemPrefab);
            stem.transform.position = position;
            stem.transform.rotation = direction;
            //stem.transform.localScale = new Vector3(StemWidth, StemLength, StemWidth);
            stem.transform.localScale = new Vector3(StemWidth, 0, StemWidth);
            stem.name = "Stem";

            var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();
            stem.transform.parent = plant.transform;

            var stemEntity = _ecsWorld.NewEntity();
            ref var stemComponent = ref stemEntity.Get<StemComponent>();

            stemComponent.Lifetime = 0;
            stemComponent.Position = position;
            stemComponent.Height = 0;
            stemComponent.MaxHeight = StemLength;
            stemComponent.Width = StemWidth;
            stemComponent.MaxWidth = StemWidth;
            stemComponent.stemGO = stem;
        }

        private void DrawLeaf(Vector3 position, Quaternion direction)
        {
            var leaf = GameObject.Instantiate(StrawberryPrefabs.LeafPrefab);
            leaf.transform.localScale = new Vector3(1f, 0, 0);
            leaf.transform.rotation = direction;
            leaf.name = "Leaf";

            var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();
            leaf.transform.parent = plant.transform;
            leaf.transform.position = plant.transform.position;

            var leafEntity = _ecsWorld.NewEntity();
            ref var component = ref leafEntity.Get<LeafComponent>();

            component.Lifetime = 0;
            component.Height = 0;
            component.Width = 0;
            component.MaxHeight = LeafLength;
            component.MaxWidth = LeafLength;
            component.LeafGO = leaf;

            component.TargetPosition = position;
        }


        private void DrawFlower(Vector3 position, Quaternion direction)
        {
            GameObject flower = GameObject.Instantiate(StrawberryPrefabs.BudPrefab);

            flower.transform.localScale = new Vector3(1f, 0, 0);

            Quaternion rotation = Quaternion.Euler(0, direction.eulerAngles.y, direction.eulerAngles.z);
            flower.transform.rotation = rotation;

            flower.name = "Flower";

            var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();
            flower.transform.parent = plant.transform;
            flower.transform.position = plant.transform.position;

            var entity = _ecsWorld.NewEntity();
            ref var component = ref entity.Get<FlowerComponent>();
            component.Lifetime = 0;
            component.FlowerGO = flower;
            component.IsBud = true;
            component.maxSize = flowerSize;

            component.TargetPosition = position;
        }

        private struct Turtle
        {
            public Quaternion direction;
            public Vector3 position;
            public Vector3 step;

            public Turtle(Turtle other)
            {
                this.direction = other.direction;
                this.position = other.position;
                this.step = other.step;
            }

            public Turtle(Quaternion direction, Vector3 position, Vector3 step)
            {
                this.direction = direction;
                this.position = position;
                this.step = step;
            }

            public void Forward()
            {
                position += direction * step;
            }

            public void RotateX(float angle)
            {
                direction *= Quaternion.Euler(angle, 0, 0);
            }

            public void RotateY(float angle)
            {
                direction *= Quaternion.Euler(0, angle, 0);
            }

            public void RotateZ(float angle)
            {
                direction *= Quaternion.Euler(0, 0, angle);
            }
        }

        private struct TransformData
        {
            public Vector3 position;
            public Quaternion rotation;

            public TransformData(Vector3 pos, Quaternion rot)
            {
                position = pos;
                rotation = rot;
            }
        }
    }
}

