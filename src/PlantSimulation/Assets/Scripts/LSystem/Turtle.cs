using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Assets.Scripts
{
    public class Turtle
    {
        private class TurtleTransform
        {
            public Vector3 Position { get; }
            public Quaternion Rotation { get; }


            public TurtleTransform(Vector3 position, Quaternion rotation)
            {
                Position = position;
                Rotation = rotation;

            }
        }

        public Vector3 Position { get; private set; }

        public Quaternion Rotation { get; private set; }

        private Stack<TurtleTransform> stack = new Stack<TurtleTransform>();

        public Turtle(Vector3 position)
        {
            Position = position;
        }

        public void Translate(Vector3 delta)
        {
            delta = Rotation * delta;

            Debug.DrawLine(Position, Position + delta, Color.black, 100f);

            Position += delta;
        }

        public void Rotate(Vector3 delta) => Rotation = Quaternion.Euler(Rotation.eulerAngles + delta);
        public void Push() => stack.Push(new TurtleTransform(Position, Rotation));
        public void Pop()
        {
            var poppedTransform = stack.Pop();
            Position = poppedTransform.Position;
            Rotation = poppedTransform.Rotation;
        }
    }
}
