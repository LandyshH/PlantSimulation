using Assets.Scripts.Systems.Lychnis;
using Leopotam.Ecs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.LSystem
{
    public class MintGenerationSystem : IEcsRunSystem
    {
        private readonly EcsWorld _ecsWorld;

        private StaticData staticData;
        private EnvironmentSettings environment;
        public MintPrefabs MintPrefabs;

        private float StemLength = 0.01f;
        private float StemWidth = 0.2f;
        private float flowerSize = 0.3f;
        private float LeafLength = 0.3f;
        private float LeafWidth = 0.3f;
        private int generations = 7;

        private int LeafCount = 0;
        private int StemCount = 0;
        //private int FlowerCount = 0;

        private float angle = 45;
        private float leafAngle = 60;

        private string axiom = "A";

        private Stack<Turtle> stack = new Stack<Turtle>();

        private Dictionary<string, List<string>> ruleset = new Dictionary<string, List<string>>()
{           // обыграть количество цветов
            {"F", new List<string>{"FF" }}, //R - random angle "F[RL]"
            {"A", new List<string>{"F[SL][DL][-Fa][+Fa]FA"}}, //"F[-L][+L]FA", 
            {"a", new List<string>{"F[SL][DL]Fb"}}, //"F[SL][DL]Fa"
            {"b", new List<string>{"F[+K][-K]Fb", "F[+K][-K][++K][--K]Fb"}}
};

        /* {"a", "I[L]a" },
     {"a", "I[L]A" },
     { "A", "I[L][b]A" },
     { "A", "I[L][b]B" },
     { "b", "I[L]b" },
     { "b", "I[L]B" },
     { "B", "I[K]B" },*/
        public void Run()
        {
            if (staticData.PlantGrowthStage == Enum.PlantGrowthStage.Embryonic && !staticData.SproutGenerated)
            {
                StemLength = 0.2f;
                StemWidth = 0.3f;
                flowerSize = 0.1f;
                LeafLength = 0.5f;
                LeafWidth = 0.5f;

                generations = 1;
                GenerateAndDrawLSystem();

                staticData.SproutGenerated = true;
                return;
            }

            if (staticData.PlantGrowthStage == Enum.PlantGrowthStage.Juvenile && !staticData.JuvnileGenerated)
            {
                var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();

                foreach (Transform child in plant.transform) Object.Destroy(child.gameObject);

                StemLength = 0.01f;
                StemWidth = 0.2f;
                flowerSize = 0.3f;
                LeafLength = 0.3f;
                LeafWidth = 0.3f;

                generations = 7;
                GenerateAndDrawLSystem();

                staticData.JuvnileGenerated = true;
                return;
            }

            if (staticData.JuvnileGenerated)
            {
                return;
            }
        }


        private void CalculateGrowth()
        {
            if (environment.Water == Enum.Water.Lack)
            {
                // - размер цветка, листьев, тыры пыры
            }

            if (environment.Water == Enum.Water.Excess)
            {
                // - размер цветка, листьев, тыры пыры
            }

            StemLength = 0.01f;
            StemWidth = 0.007f;
            flowerSize = 0.02f;
            LeafLength = 0.01f;
            LeafWidth = 0.007f;
        }

        private void GenerateAndDrawLSystem()
        {
            string result = GenerateLSystem(axiom);
            Debug.Log(result);

            DrawLSystem(result);
        }

        private string GenerateLSystem(string axiom)
        {
            string result = axiom;
            for (int i = 0; i < generations; i++)
            {
                result = GenerateNextString(result);
            }

            return result;
        }

        string GenerateNextString(string input)
        {
            string output = "";
            foreach (char c in input)
            {
                if (ruleset.ContainsKey(c.ToString()))
                {
                    List<string> possibleRules = ruleset[c.ToString()];
                    string selectedRule = possibleRules[Random.Range(0, possibleRules.Count)];
                    output += selectedRule;
                }
                else
                {
                    output += c.ToString();
                }
            }
            return output;
        }


        private void DrawLSystem(string lSystemString)
        {
            var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();

            Turtle turtle = new Turtle(plant.transform.rotation, plant.transform.position, Vector3.up * StemLength);

            var stemLengthCounter = 0;

            for (int i = 0; i < lSystemString.Length; i++)
            {
                char c = lSystemString[i];

                switch (c)
                {
                    case 'F':
                        DrawStem(turtle.position, turtle.direction);
                        turtle.Forward();
                        stemLengthCounter++;
                        break;
                    case 'L':
                        DrawLeaf(turtle.position, turtle.direction);
                        break;
                    case 'A':
                        DrawFlower(turtle.position, turtle.direction);
                        break;
                    case 'K':
                        DrawFlower(turtle.position, turtle.direction);
                        break;
                    case 'b':
                        DrawFlower(turtle.position, turtle.direction);
                        break;
                    case '+':
                        turtle.RotateZ(Random.Range(angle - 15, angle + 15)); 
                        turtle.RotateX(Random.Range(-45, 45)); 
                        break;
                    case '-':
                        turtle.RotateZ(Random.Range(-angle - 15, -angle + 15)); 
                        turtle.RotateX(Random.Range(-45, 45)); 
                        break;
                    case 'D':
                        turtle.RotateY(leafAngle);
                        turtle.RotateY(Random.Range(0, 90));
                        break;
                    case 'S':
                        turtle.RotateY(-leafAngle);
                        turtle.RotateY(Random.Range(-90, 0));
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
            if(StemWidth > 0)
                StemWidth -= StemCount * 0.0000004f;
            StemCount++;
            var stem = GameObject.Instantiate(MintPrefabs.StemPrefab);
            stem.transform.position = position;
            stem.transform.rotation = direction;
            stem.transform.localScale = new Vector3(StemWidth, StemLength, StemWidth);
            stem.name = "Internode";

            var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();
            stem.transform.parent = plant.transform;

            var stemEntity = _ecsWorld.NewEntity();
            ref var stemComponent = ref stemEntity.Get<StemComponent>();

            stemComponent.Lifetime = 0;
            stemComponent.Position = position;
            stemComponent.Height = 0;
            stemComponent.MaxHeight = StemLength;
            stemComponent.Width = StemWidth;
            stemComponent.MaxWidth = 0.01f;
            stemComponent.stemGO = stem;
        }

        private void DrawLeaf(Vector3 position, Quaternion direction)
        {
            if (LeafCount > 26) return;

            LeafCount++;

            var leaf = GameObject.Instantiate(MintPrefabs.LeafPrefab);
            leaf.transform.localScale = new Vector3(LeafWidth, 1f, LeafLength);
            leaf.transform.position = position;
            leaf.transform.rotation = direction;
            leaf.name = "Leaf " + LeafCount;

            var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();
            leaf.transform.parent = plant.transform;

            var leafEntity = _ecsWorld.NewEntity();
            ref var component = ref leafEntity.Get<LeafComponent>();

            component.Lifetime = 0;
            component.Height = 0;
            component.Width = 0;
            component.MaxHeight = 0.3f;
            component.MaxWidth = 0.03f;
            component.LeafGO = leaf;
        }

        private void DrawFlower(Vector3 position, Quaternion direction)
        {
            GameObject flower;
            if (staticData.PlantGrowthStage == Enum.PlantGrowthStage.Juvenile)
            {
                flower = GameObject.Instantiate(MintPrefabs.BudPrefab);
            }
            else
            {
                flower = GameObject.Instantiate(MintPrefabs.FlowerPrefab);
            }

            flower.transform.localScale = new Vector3(flowerSize, flowerSize, flowerSize);
            flower.transform.position = position;
            flower.transform.rotation = direction;
            flower.name = "Flower";

            var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();
            flower.transform.parent = plant.transform;

            var entity = _ecsWorld.NewEntity();
            ref var component = ref entity.Get<FlowerComponent>();
            component.FlowerGO = flower;
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