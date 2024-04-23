using Leopotam.Ecs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.LSystem
{
    public class MintGenerationSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld;

        private StaticData staticData;
        private EnvironmentSettings environment;

        private float StemLength = 0.03f;
        private float StemWidth = 0.007f;
        private float flowerSize = 0.02f;
        private int generations = 5;

        private float angle = 45;
        private float leafAngle = 60;

        private string axiom = "A";

        private Stack<Turtle> stack = new Stack<Turtle>();

        private Dictionary<string, List<string>> ruleset = new Dictionary<string, List<string>>()
{
            {"F", new List<string>{"FF" }}, //R - random angle "F[RL]"
            {"A", new List<string>{"F[SL][DL][-Fa][+Fa]FA"}}, //"F[-L][+L]FA", 
            {"a", new List<string>{"F[SL][DL]Fb"}}, //"F[SL][DL]Fa"
            {"b", new List<string>{"F[SL][DL][+FK]F[-FK]Fb"}}
};

        /* {"a", "I[L]a" },
     {"a", "I[L]A" },
     { "A", "I[L][b]A" },
     { "A", "I[L][b]B" },
     { "b", "I[L]b" },
     { "b", "I[L]B" },
     { "B", "I[K]B" },*/
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
                    string selectedRule = possibleRules[Random.Range(0, possibleRules.Count)]; // Выбираем случайное правило из списка возможных
                    output += selectedRule;
                }
                else
                {
                    output += c.ToString();
                }
            }
            return output;
        }




        /*{"a", "F[-L][+L]Fa" }, 
            {"a", "F[-L][-A][+A][+L]Fa" }, 
            {"A", "F[K]FA" },*/
        private void DrawLSystem(string lSystemString)
        {
            var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();

            Turtle turtle = new Turtle(plant.transform.rotation, plant.transform.position, 
                Vector3.up * StemLength);


            for (int i = 0; i < lSystemString.Length; i++)
            {
                char c = lSystemString[i];

                switch (c)
                {
                    case 'F':
                        DrawStem(turtle.position, turtle.direction);
                        turtle.Forward();
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
                        turtle.RotateZ(angle);
                        break;
                    case '-':
                        turtle.RotateZ(-angle);
                        break;
                    case 'D':
                        turtle.RotateZ(leafAngle);
                        break;
                    case 'S':
                        turtle.RotateZ(-leafAngle);
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
            GameObject stem = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            stem.transform.position = position;
            stem.transform.rotation = direction;
            stem.transform.localScale = new Vector3(StemWidth, StemLength, StemWidth);
            stem.name = "Internode";

            var plant = GameObject.FindGameObjectsWithTag("Plant").FirstOrDefault();
            stem.transform.parent = plant.transform;
            Renderer leafRenderer = stem.GetComponent<Renderer>();
            leafRenderer.material.color = Color.green;

            var stemEntity = _ecsWorld.NewEntity();
            ref var stemComponent = ref stemEntity.Get<StemComponent>();

            /* stemComponent.Lifetime = 0;
             stemComponent.Position = position;
             stemComponent.Height = 0;
             stemComponent.MaxHeight = StemLength * stemLengthCounter;
             stemComponent.Width = StemWidth;
             stemComponent.MaxWidth = 0.02f;
             stemComponent.stemGO = stem;*/

            //var maxScale = new Vector3(StemWidth, stemComponent.MaxHeight, StemWidth);
            //stem.transform.localScale = Vector3.Lerp(stem.transform.localScale, maxScale, Time.deltaTime);
        }

        private void DrawLeaf(Vector3 position, Quaternion direction)
        {
            GameObject leaf = GameObject.CreatePrimitive(PrimitiveType.Cube);

            leaf.transform.localScale = new Vector3(0.03f, 0.001f, 0.1f);
            //leaf.transform.localScale = new Vector3(0f, 0.0f, 0f);
            leaf.transform.position = position;
            leaf.transform.rotation = direction;
            leaf.name = "Leaf ";

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

            /*component.Lifetime = 0;
            component.Size = 0;
            component.maxSize = flowerSize;
            component.FlowerGO = flower;*/
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
