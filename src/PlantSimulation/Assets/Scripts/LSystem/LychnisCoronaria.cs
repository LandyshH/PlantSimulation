using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class LychnisCoronaria : MonoBehaviour
    {
        [SerializeField] private int iteration = 10;
        [SerializeField] private float angle = 45;
        [Range(0, 100)] public int speed = 100;

        public GameObject apicalBudPrefab;
        public GameObject leafPrefab;
        public GameObject flowerPrefab;

        private string axiom = "A";
        private int age = 0;


        private Dictionary<string, string> ruleset = new Dictionary<string, string>()
        {
            {"A(7)", "I[A(0)][A(4)]IK(0)"},
            //{$"A({age})", $"A({age + 1})" },
            {"L", "L" },
            {"K", "K" },

        };

        private Dictionary<string, Action<Turtle>> _turtleCommands = new()
        {
            {"F", turtle => turtle.Translate(new Vector3(0, 0.1f, 0)) },
            {"+", turtle => turtle.Rotate(new Vector3(25f, 0, 0))},
            {"-", turtle => turtle.Rotate(new Vector3(-25f, 0, 0))},
            {"[", turtle => turtle.Push()},
            {"]", turtle => turtle.Pop()},
        };


        private void Start()
        {
           // var lSystem = new LSystem(axiom, ruleset, _turtleCommands, transform.position);
           // lSystem.GenerateSentence();
           // lSystem.DrawSystem();
        }
    }
}
