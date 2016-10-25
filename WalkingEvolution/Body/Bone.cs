using SFML.System;
using SFML.Graphics;
using System;

public class Bone
{
    private Joint A, B;

    private float desired;
    private float size;
    private float strength;

    private RectangleShape shape;

    private Bone(Joint a, Joint b, float l, float s)
    {
        A = a;
        B = b;
        desired = 50;
        //strength = s;
        //size = (float)Math.Sqrt(strength) / 5f;

        strength = Utils.RandomFloat(10, 20);
        size = 1;

        shape = new RectangleShape();
        shape.FillColor = Color.Green;
        shape.OutlineColor = Color.White;
        shape.OutlineThickness = 1;
    }

    public static Bone ExplicitBone(Joint _A, Joint _B)
    {
        if (Joint.Distance(_A, _B) > (_A.Size + _B.Size))
        {
            return new Bone(
                _A,
                _B,
                Joint.Distance(_A, _B) * 1.1f,
                Utils.RandomFloat(10f, 100f)
                );

        }
        else
        {
            return new Bone(
                _A,
                _B,
                (_A.Size + _B.Size) * 1.1f,
                Utils.RandomFloat(10f, 100f)
                );

        }
    }

    public static Bone RandomBone(Joint _A, Joint _B)
    {
        return new Bone(
            _A,
            _B,
            Utils.RandomFloat((_A.Size + _B.Size) * 1.1f, 128f),
            Utils.RandomFloat(10f, 100f)
            );

        /*
        if(Joint.Distance(_A, _B) > (_A.Size + _B.Size))
        {
            return new Bone(
                _A,
                _B,
                Utils.RandomFloat(Joint.Distance(_A, _B) * 1.1f, 25.0f),
                Utils.RandomFloat(10f, 100f)
                );

        } else
        {
            return new Bone(
                _A,
                _B,
                Utils.RandomFloat((_A.Size + _B.Size) * 1.1f, 25.0f),
                Utils.RandomFloat(10f, 100f)
                );

        }
        */

    }

    public void step()
    {
        float distance = Joint.Distance(A, B);
        Vector2f delta = Joint.Delta(A, B);
        Vector2f normal = delta / distance;

        normal *= strength * (distance - desired) * (float)Math.Abs(distance - desired);

        A.addForce(normal);
        B.addForce(-normal);
    }

    public void draw(Vector2f shift, float scale)
    {
        float distance = Joint.Distance(A, B) * scale;
        Vector2f delta = Joint.Delta(A, B) * scale;

        shape.Size = new Vector2f(distance, size);
        shape.Origin = new Vector2f(0, size / 2);
        shape.Position = A.Position * scale;
        shape.Rotation = (float)(Math.Atan2(delta.Y, delta.X) / Math.PI) * 180f;

        Utils.Render.Draw(shape);
    }
}