using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class LSystem
{
    private string _sentence;
    private Dictionary<string, string> _ruleset;
    private Dictionary<string, Action<Turtle>> _turtleCommands;
    private Turtle _turtle;

    public LSystem(string axiom, Dictionary<string, string> ruleset, Dictionary<string, Action<Turtle>> turtleCommands, Vector3 initialPosition)
    {
        _sentence = axiom;
        _ruleset = ruleset;
        _turtleCommands = turtleCommands;

        _turtle = new Turtle(initialPosition);
    }
    public void DrawSystem()
    {
        foreach(var instruction in _sentence)
        {
            if (_turtleCommands.TryGetValue(instruction.ToString(), out var command))
            {
                command(_turtle);
            }
        }
    }

    public string GenerateSentence()
    {
        _sentence = IterateSentence(_sentence);
        return _sentence;
    }

    private string IterateSentence(string sentence)
    {
        var newSentence = "";

        foreach (var c in newSentence)
        {
            if (_ruleset.TryGetValue(c.ToString(), out var replacement))
            {
                newSentence += replacement;
            }
            else
            {
                newSentence += c;
            }
        }

        return newSentence;
    }
}

