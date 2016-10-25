using System.Diagnostics;
using System.Threading;
using System;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

public static class Utils
{
    private static Stopwatch programTime, frameTime, renderTime;
    private static long programTimeMilliseconds, frameTimeMilliseconds;
    private static float programTimeSeconds, frameTimeSeconds, renderTimeSeconds;
    private static RenderWindow programWindow;
    private static RenderTexture programTexture;
    private static Sprite renderSprite;
    private static Random programRandom;
    private static int skippedFrames = 0;
    private static float scale = 0.0001f;    

    #region ACCESSORS
    public static long ProgramTimeMilliseconds
    {
        get
        {
            return programTimeMilliseconds;
        }
    }

    public static long FrameTimeMilliseconds
    {
        get
        {
            return frameTimeMilliseconds;
        }
    }

    public static float ProgramTimeSeconds
    {
        get
        {
            return programTimeSeconds;
        }
    }

    public static float FrameTimeSeconds
    {
        get
        {
            return frameTimeSeconds;
        }
    }

    public static Random ProgramRandom
    {
        get
        {
            return programRandom;
        }
    }

    public static RenderTexture Render
    {
        get
        {
            return programTexture;
        }
    }

    public static RenderWindow Window
    {
        get
        {
            return programWindow;
        }
    }

    public static float RenderTimeSeconds
    {
        get
        {
            return renderTimeSeconds;
        }
    }

    public static float Scale
    {
        get
        {
            return scale;
        }
    }

    #endregion

    public static Vector2f RenderCenter = new Vector2f(512, 512);

    public static float RandomFloat(float min = -1.0f, float max = 1.0f)
    {
        return ((float)programRandom.NextDouble()) * (max - min) + min;
    }

    public static float RandomAngle()
    {
        return RandomFloat() * (float)Math.PI;
    }

    public static void Init()
    {
        programTime = new Stopwatch();
        frameTime = new Stopwatch();
        renderTime = new Stopwatch();

        programTime.Start();
        frameTime.Start();
        renderTime.Start();

        programRandom = new Random();

        programWindow = new RenderWindow(new VideoMode(1024, 1024), "YAY! This works!");
        programTexture = new RenderTexture(1024, 1024);
        renderSprite = new Sprite(programTexture.Texture);
        renderSprite.Rotation = 0f;
        //renderSprite.Position = new Vector2f(1024, 1024);
    }

    public static void Tick()
    {
        skippedFrames++;

        programTimeMilliseconds = programTime.ElapsedMilliseconds;
        frameTimeMilliseconds = frameTime.ElapsedMilliseconds;

        renderTimeSeconds = ((float)renderTime.ElapsedMilliseconds) / 1000f;
        programTimeSeconds = (float)programTimeMilliseconds / 1000f;
        frameTimeSeconds = (float)frameTimeMilliseconds / 1000f;

        frameTime.Restart();
    }

    public static void Tock()
    {
        while(frameTime.ElapsedTicks < 300)
        {
            Thread.SpinWait(1);
        }
    }

    public static void ClearDisplay()
    {
        Window.DispatchEvents();

        Render.Clear(Color.Black);
        DrawGrid();
    }

    public static void Display()
    {
        Render.Display();

        Window.SetTitle("Simple Creature Sim - Frames: " + skippedFrames);
        skippedFrames = 0;

        Window.Clear(Color.Black);

        Window.Draw(renderSprite);

        Window.Display();

        renderTime.Restart();
    }

    private static void DrawGrid()
    {
        Vertex[] vArr = new Vertex[2];
        vArr[0].Color = Color.White;
        vArr[1].Color = Color.White;

        for (int x = 0; x < 32; x++)
        {
            vArr[0].Position = new Vector2f(x * 32 + 16, 0);
            vArr[1].Position = new Vector2f(x * 32 + 16, 1024);
            Utils.Render.Draw(vArr, PrimitiveType.Lines);

            vArr[0].Position = new Vector2f(0, x * 32 + 16);
            vArr[1].Position = new Vector2f(1024, x * 32 + 16);
            Utils.Render.Draw(vArr, PrimitiveType.Lines);
        }
    }
}