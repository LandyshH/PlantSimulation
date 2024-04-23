using Assets.Scripts.Providers;
using Leopotam.Ecs;
using System.Collections.Generic;
using UnityEngine;

public partial class LSystemGenerator : MonoBehaviour
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
    public void ClearLSystem()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
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
        for (int i = 0; i < generations + 1; i++)
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
        {
            return $"FI({internodeLength})[SL(0)][-A(0)][DL(0)][+A(4)]FI({internodeLength * 0.5})[K(0)]";
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
        Turtle turtle = new Turtle(transform.rotation, transform.position, Vector3.up * StemLength);
        var stemLengthCounter = 0;

        foreach (char c in lSystemString)
        {
            switch (c)
            {
                case 'F':
                    stemLengthCounter++;
                    break;
                case 'I':
                    DrawStem(turtle.position, turtle.direction, stemLengthCounter);
                    for (var i = 0; i < stemLengthCounter; i++) turtle.Forward();
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
                    DrawLeaf(turtle.position, turtle.direction);
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
        //stem.transform.localScale = Vector3.zero;
        stem.transform.localScale = new Vector3(StemWidth, StemLength * stemLengthCounter, StemWidth);
        stem.name = "Internode";

        stem.transform.parent = transform;
        Renderer leafRenderer = stem.GetComponent<Renderer>();
        leafRenderer.material.color = Color.green;
        stem.AddComponent<StemProvider>();
    }

    private void DrawLeaf(Vector3 position, Quaternion direction)
    {
        GameObject leaf = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leaf.transform.localScale = new Vector3(0.1f, 0.01f, 0.1f);
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
}

