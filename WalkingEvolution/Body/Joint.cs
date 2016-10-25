using SFML.System;
using SFML.Graphics;
using System;
using System.Collections.Generic;

public class Joint
{
    private static int jointCount;

    private Vector2f position;
    private Vector2f velocity;
    private Vector2f force;
    private float weight;
    private float size;
    private CircleShape shape;
    private int id;
    private Dictionary<int, Joint> connections;

    private Joint(Joint connection)
    {
        weight = 5;
        size = (float)Math.Sqrt(weight);

        //float length = Utils.RandomFloat((size + connection.Size) * 1.1f, 128f);
        float length = 50;
        float angle = Utils.RandomAngle();

        //Console.WriteLine(angle);
        //Console.WriteLine(length + " : " + (size + connection.Size));

        position = connection.Position;
        position.X += (float)Math.Cos(angle) * length;
        position.Y += (float)Math.Sin(angle) * length;

        id = jointCount++;

        connections = new Dictionary<int, Joint>();
        force = new Vector2f();
        shape = new CircleShape();
        shape.OutlineColor = Color.White;
        shape.OutlineThickness = 1;
        shape.FillColor = Color.Blue;
        shape.Radius = size;
        shape.Origin = new Vector2f(size, size);
    }

    private Joint()
    {
        Position = new Vector2f(512, 512);
        weight = 5;

        id = jointCount++;

        connections = new Dictionary<int, Joint>();
        force = new Vector2f();
        size = (float)Math.Sqrt(weight);
        shape = new CircleShape();
        shape.OutlineColor = Color.White;
        shape.OutlineThickness = 1;
        shape.FillColor = Color.Blue;
        shape.Radius = size;
        shape.Origin = new Vector2f(size, size);
    }

    public bool tryAddConnection(Joint j)
    {
        if (connections.ContainsKey(j.Id) || j.Id == this.id)
        {
            return false;
        } else
        {
            connections.Add(j.Id, j);
            j.tryAddConnection(this);
            return true;
        }
    }

    public Vector2f Position
    {
        get
        {
            return position;
        }
        
        set
        {
            position = value;
        }
    }

    public int Id
    {
        get
        {
            return id;
        }
    }

    public float Size
    {
        get
        {
            return size;
        }

        set
        {
            size = value;
        }
    }

    public static float Distance(Joint A, Joint B)
    {
        float dist = (float)Math.Sqrt(Math.Pow(A.position.X - B.position.X, 2) + Math.Pow(A.position.Y - B.position.Y, 2));
        return dist > 0.0001f ? dist : 0.0001f;
    }

    public static Vector2f Delta(Joint A, Joint B)
    {
        return new Vector2f(B.position.X - A.position.X, B.position.Y - A.position.Y);
    }

    public static Joint anchor()
    {
        return new Joint();
    }

    public static Joint random(Joint j)
    {
        Joint i = new Joint(j);
        i.tryAddConnection(j);
        return i;
    }

    public static void CheckIntersection(Joint A, Joint B)
    {
        float distance = Joint.Distance(A, B);
        if(distance < (A.Size + B.Size))
        {
            Vector2f v = Joint.Delta(A, B) / Joint.Distance(A, B);
            A.addForce(-v * 100f);
            B.addForce(v * 100f);
        }
    }

    public void addForce(Vector2f f)
    {
        force += f;
        //Console.WriteLine(id + " - " + f.ToString() + " && " + force.ToString());
    }

    public Vector2f calculate()
    {
        Vector2f gravity = new Vector2f(0, 100);

        Vector2f delta = force * Utils.Scale;
        
        velocity += delta / weight;

        velocity -= velocity * Utils.Scale; // Drag;

        velocity += gravity * Utils.Scale;

        Vector2f newPosition = position + velocity * Utils.Scale;
        force *= 0;

       /* if (newPosition.X < 0 && velocity.X < 0)
        {
            velocity.X *= -0.9f;
        }
        else */
        
        if (newPosition.Y > 1024 && velocity.Y > 0)
        {
            velocity.Y *= -0.999f;
        }

        position += velocity * Utils.Scale;

        return Position;
    }

    public void draw(Vector2f shift, float scale)
    {
        position -= shift;
        shape.Position = position  * scale;
        Utils.Render.Draw(shape);
    }
}