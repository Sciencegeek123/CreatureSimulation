using SFML.System;
using SFML.Graphics;
using System;

public class Muscle
{

    private float duration;
    private int currentStep = 0;
    private float stepDuration;
    private float[] strengths;
    private RectangleShape shape;

    private Joint A, B;

    private Muscle(Joint _A, Joint _B)
    {
        A = _A;
        B = _B;

        strengths = new float[Utils.ProgramRandom.Next(8, 256)];
        for(int i = 0; i < strengths.Length; i++)
        {
            strengths[i] = (float)Utils.RandomFloat(-1000, 1000);
        }

        duration = Utils.RandomFloat(0.1f, 100f) / (float)(strengths.Length + 1);
        stepDuration = duration;
        shape = new RectangleShape();
        shape.FillColor = Color.Red;
        shape.OutlineColor = Color.White;
        shape.OutlineThickness = 1;
    }

    public static Muscle AddRandomMuscle(Joint A, Joint B)
    {
        return new Muscle(A, B);
    }

    public void step()
    {
        stepDuration -= Utils.Scale;
        if(stepDuration < 0)
        {
            stepDuration = duration;
            currentStep = ++currentStep % strengths.Length;
        }

        float thisStep = strengths[currentStep];
        float nextStep = strengths[(currentStep + 1) % strengths.Length];

        float value = thisStep + (nextStep - thisStep) * (stepDuration - duration) / duration;

        value = thisStep;

        float distance = Joint.Distance(A, B);
        Vector2f delta = Joint.Delta(A, B);
        Vector2f force = (delta / distance) * value;

        A.addForce(-force);
        B.addForce(force);
    }

    public void draw(Vector2f shift, float scale)
    {
        float distance = Joint.Distance(A, B) * scale;
        Vector2f delta = Joint.Delta(A, B) * scale;

        shape.Size = new Vector2f(distance, 1);
        shape.Origin = new Vector2f(0, 0.5f);
        shape.Position = A.Position * scale;
        shape.Rotation = (float)(Math.Atan2(delta.Y, delta.X) / Math.PI) * 180f;

        Utils.Render.Draw(shape);
    }
}