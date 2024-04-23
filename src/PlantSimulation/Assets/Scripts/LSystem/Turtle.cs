using UnityEngine;


public struct Turtle
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

