using Leopotam.Ecs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.LSystem
{
    public class LychnisGenerationSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld;

        //public LychnisCoronariaPrefabs LychnisCoronariaPrefabs;
        private StaticData staticData;
        private EnvironmentSettings environment;

        private float angle = 45;
        private float internodeLength = 20;

        private string axiom = "A(7)";

        private Stack<Turtle> stack = new Stack<Turtle>();

        private float StemLength = 0.01f;
        private float StemWidth = 0.005f;
        private float flowerSize = 0.05f;

        private int leafNumber = 0;


        public void Init()
        {
            GenerateAndDrawLSystem();
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
            for (int i = 0; i < staticData.Generations + 1; i++)
            {
                result = ApplyRules(result);
            }

            return result;
        }

        private string ApplyRules(string input)
        {
            string output = "";
            for (int i = 0; i < input.Length; i++)
            {
                char currentChar = input[i];

                if (currentChar == 'A' || currentChar == 'I' || currentChar == 'L' || currentChar == 'K')
                {
                    if (i + 4 < input.Length && input[i + 1] == '(' && char.IsDigit(input[i + 2]) && char.IsDigit(input[i + 3]) && input[i + 4] == ')')
                    {
                        int t = int.Parse(input.Substring(i + 2, 2));

                        switch (currentChar)
                        {
                            case 'A':
                                output += ApplyRuleA(t);
                                break;
                            case 'I':
                                output += ApplyRuleI(t);
                                break;
                            case 'L':
                                output += ApplyRuleL(t);
                                break;
                            case 'K':
                                output += ApplyRuleK(t);
                                break;
                        }

                        i += 4;
                    }
                    else if (i + 3 < input.Length && input[i + 1] == '(' && char.IsDigit(input[i + 2]) && input[i + 3] == ')')
                    {
                        int t = int.Parse(input.Substring(i + 2, 1));

                        switch (currentChar)
                        {
                            case 'A':
                                output += ApplyRuleA(t);
                                break;
                            case 'I':
                                output += ApplyRuleI(t);
                                break;
                            case 'L':
                                output += ApplyRuleL(t);
                                break;
                            case 'K':
                                output += ApplyRuleK(t);
                                break;
                        }

                        i += 3;
                    }

                    else
                    {
                        output += currentChar;
                    }
                }
                else if (currentChar == '<' || currentChar == '>')
                {
                    output += "";
                }
                else
                {
                    output += currentChar;
                }
            }
            return output;
        }



        //FI(20)[&(60)∼L(0)]/(90)[&(45)A(0)]/(90)[&(60)∼L(0)]/(90)[&(45)A(4)]FI(10)∼K(0)
        // F - вперед 
        // L(t) - листок на 60 град от стебля
        // A(t) новое ветвление на 45 град от стебля

        private string ApplyRuleA(int t)
        {
            if (t == 7)
            {
                return $"<FI({internodeLength})[SL(0)][-A(0)][DL(0)][+A(4)]FI({internodeLength * 0.5})[K(0)]>";
            }
            else
            {
                return $"A({t + 1})";
            }

        }

        private string ApplyRuleI(int t)
        {
            string output = "";

            for (int i = 0; i < t; i++)
            {
                output += "F";

                if (i == t - 1) output += "I";
            }

            return output;
        }

        private string ApplyRuleL(int t)
        {
            return $"L({t + 1})";
        }

        private string ApplyRuleK(int t)
        {
            return $"K({t + 1})";
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
                        stemLengthCounter++;
                        break;
                    case 'I':
                        DrawStem(turtle.position, turtle.direction, stemLengthCounter);
                        for (var j = 0; j < stemLengthCounter; j++) turtle.Forward();
                        stemLengthCounter = 0;
                        break;
                    case '+':
                        turtle.RotateZ(angle);
                        break;
                    case '-':
                        turtle.RotateZ(-angle);
                        break;
                    case 'S':
                        turtle.RotateZ(60f);
                        break;
                    case 'D':
                        turtle.RotateZ(-60f);
                        break;
                    case '[':
                        stack.Push(turtle);
                        break;
                    case ']':
                        turtle = stack.Pop();
                        break;
                    case 'L':
                        int leafNumber = 0;
                        for (int k = i + 1; k < lSystemString.Length; k++)
                        {
                            if (lSystemString[k] == '(')
                            {
                                string numberString = "";
                                for (int m = k + 1; m < lSystemString.Length; m++)
                                {
                                    if (char.IsDigit(lSystemString[m]))
                                    {
                                        numberString += lSystemString[m];
                                    }
                                    else if (lSystemString[m] == ')')
                                    {
                                        leafNumber = int.Parse(numberString);
                                        i = m; // Устанавливаем i на закрывающую скобку, чтобы пропустить эту часть при следующей итерации основного цикла
                                        break;
                                    }
                                    else
                                    {
                                        break; // Выходим из внутреннего цикла, если не число и не закрывающая скобка
                                    }
                                }
                                break; // Выходим из внешнего цикла, после того как получили номер листа
                            }
                        }
                        DrawLeaf(turtle.position, turtle.direction, leafNumber);
                        break;
                    case 'K':
                        DrawFlower(turtle.position, turtle.direction);
                        break;
                }
            }
        }


        private void DrawStem(Vector3 position, Quaternion direction, int stemLengthCounter)
        {
            GameObject stem = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            stem.transform.position = position;
            stem.transform.rotation = direction;
            stem.transform.localScale = new Vector3(StemWidth, 0, StemWidth);
            stem.name = "Internode";

            var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();
            stem.transform.parent = plant.transform;
            Renderer leafRenderer = stem.GetComponent<Renderer>();
            leafRenderer.material.color = Color.green;

            var stemEntity = _ecsWorld.NewEntity();
            ref var stemComponent = ref stemEntity.Get<StemComponent>();

            stemComponent.Lifetime = 0;
            stemComponent.Position = position;
            stemComponent.Height = 0;
            stemComponent.MaxHeight = StemLength * stemLengthCounter;
            stemComponent.Width = StemWidth;
            stemComponent.MaxWidth = 0.02f;
            stemComponent.stemGO = stem;

            //var maxScale = new Vector3(StemWidth, stemComponent.MaxHeight, StemWidth);
            //stem.transform.localScale = Vector3.Lerp(stem.transform.localScale, maxScale, Time.deltaTime);
        }

        private void DrawLeaf(Vector3 position, Quaternion direction, int leafNumberB)
        {
            GameObject leaf = GameObject.CreatePrimitive(PrimitiveType.Cube);

            //leaf.transform.localScale = new Vector3(0.3f, 0.01f, 0.1f);
            leaf.transform.localScale = new Vector3(0f, 0.0f, 0f);
            leaf.transform.position = position;
            leaf.transform.rotation = direction;
            leaf.name = "Leaf " + leafNumber + " " + leafNumberB;
            leafNumber++;

            var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();
            leaf.transform.parent = plant.transform;

            Renderer leafRenderer = leaf.GetComponent<Renderer>();
            leafRenderer.material.color = Color.yellow;

            var leafEntity = _ecsWorld.NewEntity();
            ref var component = ref leafEntity.Get<LeafComponent>();

            component.Lifetime = 0;
            component.Height = 0;
            component.Width = 0;
            component.MaxHeight = 0.3f;
            component.MaxWidth = 0.03f;
            component.LeafNumber = leafNumberB;
            component.LeafGO = leaf;
        }

        private void DrawFlower(Vector3 position, Quaternion direction)
        {
            GameObject flower = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            flower.transform.localScale = new Vector3(flowerSize, flowerSize, flowerSize);
            flower.transform.position = position;
            flower.transform.rotation = direction;
            flower.name = "Flower";

            var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();
            flower.transform.parent = plant.transform;

            Renderer flowerRenderer = flower.GetComponent<Renderer>();
            flowerRenderer.material.color = Color.red;

            var entity = _ecsWorld.NewEntity();
            ref var component = ref entity.Get<FlowerComponent>();

            component.Lifetime = 0;
            component.Size = 0;
            component.maxSize = flowerSize;
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
