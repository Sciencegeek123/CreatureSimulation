using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

class Program
{
    static void Main(string[] args)
    {
        Program myProgram = new Program();
        myProgram.Start();
        while (myProgram.Update()) ;
        myProgram.End();
    }

    Body myBody;
    bool continueRunning;

    private void OnKeyPressed(Object src, KeyEventArgs args)
    {
        switch (args.Code)
        {
            case Keyboard.Key.F1:
                onJointAdd();
                break;
            case Keyboard.Key.F2:
                onBoneAdd();
                break;
            case Keyboard.Key.F3:
                onMuscleAdd();
                break;
            case Keyboard.Key.F4:
                break;
            case Keyboard.Key.F5:
                onBodyReset();
                break;
            case Keyboard.Key.F10:
                onBodyBuild();
                break;
            case Keyboard.Key.Escape:
                continueRunning = false;
                break;
            default:
                break;
        }
    }

    private void onClose(object sender, System.EventArgs e)
    {
        continueRunning = false;
    }

    private void onJointAdd()
    {
        myBody.AddRandomJoint();
    }

    private void onBoneAdd()
    {
        myBody.TryAddRandomBone();
    }

    private void onMuscleAdd()
    {
       myBody.TryAddRandomMuscle();
    }

    private void onBodyReset()
    {
        myBody = new Body();
        myBody.AddRandomJoint();
    }

    private void onBodyBuild()
    {
        myBody = new Body();

        int randomJoints = Utils.ProgramRandom.Next(3, 20);

        int linksRemaining = randomJoints * (randomJoints - 1) / 2;

        linksRemaining = Utils.ProgramRandom.Next(linksRemaining);

        int randomBones = 0, randomMuscles = 0;

        while (--linksRemaining > 0)
        {
            if(Utils.ProgramRandom.NextDouble() > 0.5f)
            {
                randomBones++;
            } else
            {
                randomMuscles++;
            }
        }


        Console.WriteLine("Generating body with " + randomJoints + " joints, " + randomBones + " bones, and " + randomMuscles + " muscles.");

        myBody.reachRequirments(randomJoints, randomBones, randomMuscles);
    }

    private void Start()
    {
        Utils.Init();

        continueRunning = true;
        
        Utils.Window.Closed += new EventHandler(onClose);
        Utils.Window.KeyPressed += new EventHandler<KeyEventArgs>(OnKeyPressed);

        onBodyReset();
    }

    private bool Update()
    {
        Utils.Tick();

        if(myBody.RequirementsUnmet)
        {
            myBody.updateRequirements();
        }

        myBody.update(); 

        if(Utils.RenderTimeSeconds > 0.05f)
        {
            Utils.ClearDisplay();
            myBody.draw();
            Utils.Display();
        } else
        {
            Utils.Tock();
        }

        return continueRunning;
    }

    private void End()
    {
        Utils.Window.Close();
    }
}

