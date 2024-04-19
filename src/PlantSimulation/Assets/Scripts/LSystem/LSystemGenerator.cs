using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class LSystemGenerator : MonoBehaviour
{
    public GameObject apicalBudPrefab;
    public GameObject leafPrefab;
    public GameObject flowerPrefab;

    [SerializeField] private float angle = 45;
    [SerializeField, Range(1, 40)] private int generations = 10;
    [SerializeField] private float internodeLength = 20;

    private string axiom = "A(7)";

    private Stack<Turtle> stack = new Stack<Turtle>();

    [SerializeField] private float StemLength = 0.01f;
    [SerializeField] private float StemWidth = 0.005f;
    [SerializeField] private float flowerSize = 0.05f;

    void Start()
    {
        GenerateAndDrawLSystem();
    }

    //private void OnValidate()
    //{
    //    ClearLSystem();
    //    GenerateAndDrawLSystem();
    //}

    private void GenerateAndDrawLSystem()
    {
        string result = GenerateLSystem(axiom);
        Debug.Log(result);

        DrawLSystem(result);
    }

    public void ClearLSystem()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private string GenerateLSystem(string axiom)
    {
        string result = axiom;
        for (int i = 0; i < generations; i++)
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
        { //FI[-FL(0)]+[-A(0)]+[-FL(0)]+[-A(4)]FIFK(0)
            return $"FI({internodeLength})[-L(0)][-A(0)][+L(0)][+A(4)]FI({internodeLength*0.5})[K(0)]";
        }
        else
        {
            return $"A({t + 1})";
        }

    }

    private string ApplyRuleI(int t)
    {
        if (t > 0)
        {
            return $"FFI({t - 1})";
        }
        else
        {
            return "I";
        }
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
        Turtle turtle = new Turtle(transform.rotation, transform.position, Vector3.up * StemLength * 2);

        foreach (char c in lSystemString)
        {
            switch (c)
            {
                case 'F':
                    DrawStem(turtle.position, turtle.direction);
                    turtle.Forward();
                    break;
                case '+':
                    turtle.RotateZ(angle); 
                    break;
                case '-':
                    turtle.RotateZ(-angle); 
                    break;
                case '[':
                    stack.Push(turtle);
                    break;
                case ']':
                    turtle = stack.Pop();
                    break;
                case 'L':
                    //turtle.RotateZ(60f);
                    DrawLeaf(turtle.position, turtle.direction);
                    break;
                case 'K':
                    DrawFlower(turtle.position, turtle.direction);
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

        stem.transform.parent = transform;

        Renderer leafRenderer = stem.GetComponent<Renderer>();
        leafRenderer.material.color = Color.green;
    }

    private void DrawLeaf(Vector3 position, Quaternion direction)
    {
        GameObject leaf = GameObject.CreatePrimitive(PrimitiveType.Cube); 
        leaf.transform.localScale = new Vector3(0.3f, 0.01f, 0.1f);
        leaf.transform.position = position; 
        leaf.transform.rotation = direction;
        leaf.name = "Leaf";
        leaf.transform.parent = transform;

        Renderer leafRenderer = leaf.GetComponent<Renderer>();
        leafRenderer.material.color = Color.yellow;
    }

    private void DrawFlower(Vector3 position, Quaternion direction)
    {
        GameObject flower = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        flower.transform.localScale = new Vector3(flowerSize, flowerSize, flowerSize);
        flower.transform.position = position; 
        flower.transform.rotation = direction; 
        flower.name = "Flower";
        flower.transform.parent = transform;

        Renderer flowerRenderer = flower.GetComponent<Renderer>();
        flowerRenderer.material.color = Color.red;
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
}


public struct TransformData
{
    public Vector3 position;
    public Quaternion rotation;

    public TransformData(Vector3 pos, Quaternion rot)
    {
        position = pos;
        rotation = rot;
    }
}

