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

        private float angle = 45;
        private float leafAngle = 60;

        private string axiom = "A";

        private Stack<Turtle> stack = new Stack<Turtle>();

        private Vector3 stemPosition;

        private Dictionary<string, List<string>> ruleset = new Dictionary<string, List<string>>()
        {
            {"F", new List<string>{"FF" }},
            {"A", new List<string>{"F[SL][DL][-Fa][+Fa]FA"}},
            {"a", new List<string>{"F[SL][DL]Fb"}},
            {"b", new List<string>{"F[+K][-K]Fb", "F[+K][-K][++K][--K]Fb","F[+K][-K][++K][--K][+K][-K]Fb"}}
        };

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

            if (staticData.PlantGrowthStage == Enum.PlantGrowthStage.Juvenile && !staticData.JuvenileGenerated)
            {
                var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();

                //foreach (Transform child in plant.transform) Object.Destroy(child.gameObject);

                StemLength = 0.005f;
                StemWidth = 0.2f;
                flowerSize = 0.3f;
                LeafLength = 0.3f;
                LeafWidth = 0.3f;

                CalculateGrowth();

                generations = 7;
                GenerateAndDrawLSystem();

                staticData.JuvenileGenerated = true;
                return;
            }

            if (staticData.JuvenileGenerated)
            {
                return;
            }
        }

        private void CalculateGrowth()
        {
            if (environment.Water == Enum.Water.Lack || environment.Water == Enum.Water.Excess)
            {
                StemLength -= 0.003f;
                StemWidth -= 0.001f;

                flowerSize -= 0.1f;

                LeafWidth -= 0.07f;
                LeafLength -= 0.07f;
            }

            if (environment.Temperature == Enum.Temperature.Max)
            {
                StemLength -= 0.005f;
                StemWidth -= 0.001f;

                flowerSize -= 0.1f;

                LeafWidth -= 0.1f;
                LeafLength -= 0.1f;
            }

            if (environment.Temperature == Enum.Temperature.Min)
            {
                StemLength -= 0.005f;
                StemWidth -= 0.001f;
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

            Turtle turtle = new Turtle(plant.transform.rotation, plant.transform.position, Vector3.up * StemLength * 2);

            var stemLengthCounter = 0;

            // Словарь для хранения начальных позиций и направлений
            var directionData = new Dictionary<Quaternion, (Vector3, int)>();

            for (int i = 0; i < lSystemString.Length; i++)
            {
                char c = lSystemString[i];

                switch (c)
                {
                    case 'F':
                        int count = 1;
                        while (i + 1 < lSystemString.Length && lSystemString[i + 1] == 'F')
                        {
                            count++;
                            i++;
                        }
                        if (directionData.ContainsKey(turtle.direction))
                        {
                            var data = directionData[turtle.direction];
                            directionData[turtle.direction] = (data.Item1, data.Item2 + count);
                        }
                        else
                        {
                            directionData[turtle.direction] = (turtle.position, count);
                        }
                        for (int j = 0; j < count; j++)
                        {
                            turtle.Forward();
                        }
                        stemLengthCounter += count;
                        break;
                    case 'L':
                        DrawLeaf(turtle.position, turtle.direction);
                        break;
                    case 'A':
                    case 'K':
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

            // Рисуем стебли из списка направлений
            foreach (var direction in directionData)
            {
                DrawStem(direction.Value.Item1, direction.Key, direction.Value.Item2);
            }
        }

        private void DrawStem(Vector3 position, Quaternion direction, int count)
        {
            float totalLength = StemLength * count;
            if (StemWidth > 0)
            {
                StemWidth -= StemCount * 0.0000004f;
            }
            StemCount++;

            var stem = GameObject.Instantiate(MintPrefabs.StemPrefab);
            stem.transform.position = position;
            stem.transform.rotation = direction;
            stem.transform.localScale = new Vector3(StemWidth, 0f, StemWidth);
            stem.name = "Stem";

            var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();
            stem.transform.parent = plant.transform;

            var stemEntity = _ecsWorld.NewEntity();
            ref var stemComponent = ref stemEntity.Get<StemComponent>();

            stemComponent.Lifetime = 0;
            stemComponent.Position = position;
            stemComponent.Height = 0;
            stemComponent.MaxHeight = totalLength;
            stemComponent.Width = StemWidth;
            stemComponent.MaxWidth = StemWidth;
            stemComponent.stemGO = stem;

            stemPosition = position;
        }

        private void DrawLeaf(Vector3 position, Quaternion direction)
        {
            if (LeafCount > 26) return;

            LeafCount++;

            var leaf = GameObject.Instantiate(MintPrefabs.LeafPrefab);

            //Vector3 leafPosition = stemPosition;

            leaf.transform.localScale = new Vector3(0, 0, 0);
            leaf.transform.rotation = direction;
            leaf.transform.position = position;
            leaf.name = "Leaf " + LeafCount;

            var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();
            leaf.transform.parent = plant.transform;

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
            GameObject flower;
            flower = GameObject.Instantiate(MintPrefabs.BudPrefab);

            flower.transform.localScale = new Vector3(0, 0, 0);
            flower.transform.position = position;
            flower.transform.rotation = direction;
            flower.name = "Flower";

            var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();
            flower.transform.parent = plant.transform;

            var entity = _ecsWorld.NewEntity();
            ref var component = ref entity.Get<FlowerComponent>();
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
            public Quaternion lastDirection;

            public Turtle(Turtle other)
            {
                this.direction = other.direction;
                this.position = other.position;
                this.step = other.step;
                this.lastDirection = other.lastDirection;
            }

            public Turtle(Quaternion direction, Vector3 position, Vector3 step)
            {
                this.direction = direction;
                this.position = position;
                this.step = step;
                this.lastDirection = direction;
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

            public bool HasDirectionChanged()
            {
                return direction != lastDirection;
            }

            public void UpdateLastDirection()
            {
                lastDirection = direction;
            }
        }
    }
}
