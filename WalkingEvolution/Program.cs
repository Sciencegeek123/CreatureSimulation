using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Diagnostics;
using System.Threading;

namespace WalkingEvolution
{
    class Program
    {
        static void Main(string[] args)
        {
            Program myProgram = new Program();
            myProgram.Start();
            while (myProgram.Update()) ;
            myProgram.End();
        }

        private Program()
        {

        }

        RenderWindow myWindow;
        CircleShape myShape;
        Random myRandom;
        Vector2f myPosition, myVelocity;
        private Stopwatch programTime, frameTime;
        bool continueRunning;

        private void onClose(object sender, System.EventArgs e)
        {
            continueRunning = false;
        }

        private void Start()
        {
            programTime = new Stopwatch();
            programTime.Start();

            frameTime = new Stopwatch();

            continueRunning = true;

            myWindow = new RenderWindow(new VideoMode(1024, 1024), "YAY! This works!");

            myWindow.Closed += new EventHandler(onClose);

            myShape = new CircleShape(10.0f);
            myShape.FillColor = Color.Blue;
            myShape.OutlineColor = Color.White;
            myShape.OutlineThickness = 2;

            myRandom = new Random();

            myPosition = new Vector2f((float)myRandom.Next(-1024, 1024) / 512.0f, (float)myRandom.Next(-1024, 1024) / 512.0f);
            myVelocity = new Vector2f((float)myRandom.Next(-1024, 1024) / 512.0f, (float)myRandom.Next(-1024, 1024) / 512.0f);
        }

        private bool Update()
        {
            long longCurrentTime = programTime.ElapsedMilliseconds;
            long longDeltaTime = frameTime.ElapsedMilliseconds;
            frameTime.Restart();

            float currentTime = (float)longCurrentTime;
            float deltaTime = (float)longDeltaTime / 1000.0f;

            Vector2f nextPosition = myPosition + myVelocity * deltaTime * 100.0f; ;

            if(nextPosition.X < 10 || nextPosition.X > 1014)
            {
                myVelocity.X *= -1;

                myVelocity.X += ((float)myRandom.Next(-256, 256) / 256.0f);

                myVelocity.Y += ((float)myRandom.Next(-256, 256) / 256.0f) * 0.1f;
            }

            if (nextPosition.Y < 10 || nextPosition.Y > 1014)
            {
                myVelocity.Y *= -1;

                myVelocity.Y += ((float)myRandom.Next(-256, 256) / 256.0f);

                myVelocity.X += ((float)myRandom.Next(-256, 256) / 256.0f) * 0.1f;
            }

            if (longDeltaTime < 5)
            {
                Thread.Sleep((int)(5 - longDeltaTime));
            }

            myPosition = myPosition + myVelocity * deltaTime * 100.0f;

            Console.WriteLine(nextPosition.ToString());

            myShape.Position = myPosition;

            myWindow.Clear(Color.Black);
            myWindow.Draw(myShape);
            myWindow.Display();

            myWindow.DispatchEvents();

            return continueRunning;
        }

        private void End()
        {
            myWindow.Close();
        }
    }
}
