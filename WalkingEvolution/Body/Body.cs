using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
public class Body
{
    List<Joint> joints;
    List<Bone> bones;
    List<Muscle> muscles;

    private bool requirementsReached;
    private int jointRequirement;
    private int boneRequirement;
    private int muscleRequirement;
    private float frameDelay;
    private const float DefaultDelay = 0.01f;

    public bool RequirementsUnmet
    {
        get
        {
            return !requirementsReached;
        }
    }

    public Body()
    {
        joints = new List<Joint>();
        bones = new List<Bone>();
        muscles = new List<Muscle>();
        requirementsReached = true;
    }

    public void reachRequirments(int j, int b, int m)
    {
        jointRequirement = j;
        boneRequirement = b;
        muscleRequirement = m;
        AddRandomJoint();
        frameDelay = DefaultDelay;
        requirementsReached = false;
    }

    public void updateRequirements()
    {
        frameDelay -= Utils.Scale;
        if(frameDelay  < 1)
        {
            if (joints.Count < jointRequirement)
            {
                AddRandomJoint();
                frameDelay = DefaultDelay;
                if (joints.Count == jointRequirement)
                {
                    Console.WriteLine("Joints Generated");
                }
            }
            else if (bones.Count < boneRequirement && muscles.Count < muscleRequirement)
            {
                if (Utils.ProgramRandom.NextDouble() > 0.5f)
                {
                    if (TryAddRandomBone())
                    {
                        frameDelay = DefaultDelay;
                        if (bones.Count == boneRequirement)
                        {
                            Console.WriteLine("Bones Generated");
                        }
                    }
                } else
                {
                    if (TryAddRandomMuscle())
                    {
                        frameDelay = DefaultDelay;
                        if (muscles.Count == muscleRequirement)
                        {
                            Console.WriteLine("Muscles Generated");
                        }
                    }
                }
             }
            else if (bones.Count < boneRequirement)
            {
                if (TryAddRandomBone())
                {
                    frameDelay = DefaultDelay;
                    if (bones.Count == boneRequirement)
                    {
                        Console.WriteLine("Bones Generated");
                    }
                }
            }
            else if (muscles.Count < muscleRequirement)
            {
                if (TryAddRandomMuscle())
                {
                    frameDelay = DefaultDelay;
                    if (muscles.Count == muscleRequirement)
                    {
                        Console.WriteLine("Muscles Generated");
                    }
                }
            }
            else
            {
                Console.WriteLine("Body Complete");
                requirementsReached = true;
            }
        }
    }

    public void update()
    {
        if (joints.Count < 1) return;
        //Push joints away from each other
        for (int i = 0; i < joints.Count; i++)
        {
            for (int j = i + 1; j < joints.Count; j++)
            {
                Joint.CheckIntersection(joints[i], joints[j]);
            }
        }

        //Simulate Bones
        foreach (Bone b in bones)
        {
            b.step();
        }

        //Simulate Muscles
        foreach (Muscle m in muscles)
        {
            m.step();
        }

        //Simulate Joints

        foreach (Joint j in joints)
        {
            j.calculate();
        }
    }

    public void draw()
    {

        float xmin, xmax, ymin, ymax;

        xmin = 0;
        ymin = 0;

        xmax = 1024;
        ymax = 1024;

        //Center Joints
        foreach (Joint j in joints)
        {
            Vector2f pos = j.calculate();

            if (pos.X < xmin)
            {
                xmin = pos.X;
            }
            else if (pos.X > xmax)
            {
                xmax = pos.X;
            }

            if (pos.Y < ymin)
            {
                ymin = pos.Y;
            }
            else if (pos.Y > ymax)
            {
                ymax = pos.Y;
            }
        }

        float scaleX = 1024f / (xmax - xmin);
        float scaleY = 1024f / (xmax - xmin);

        float scale = scaleX < scaleY ? scaleX : scaleY;
        //Console.WriteLine("Raw scale: " + scale);
        scale = scale > 1 ? 1 : scale;

        //Vector2f shift = new Vector2f(512f - (((xmin + xmax) / 2) * scale), 512f - (((ymin + ymax) / 2) * scale));
        Vector2f shift = new Vector2f(512f - (((ymin + ymax) / 2) * scale), 0);
        //Draw Joints
        foreach (Joint j in joints)
        {
            j.draw(-shift, scale);
        }

        //Draw Bones
        foreach (Bone b in bones)
        {
            b.draw(-shift, scale);
        }

        //Draw Muscles
        foreach (Muscle m in muscles)
        {
            m.draw(-shift, scale);
        }
    }

    public void AddRandomJoint()
    {
        if(joints.Count == 0)
        {
            joints.Add(Joint.anchor());
        } else
        {
            Joint jointToAdd = Joint.random(joints[Utils.ProgramRandom.Next(joints.Count)]);
            Bone boneToAdd = Bone.ExplicitBone(jointToAdd, joints[Utils.ProgramRandom.Next(joints.Count)]);
            joints.Add(jointToAdd);
            bones.Add(boneToAdd);
        }

    }

    public bool TryAddRandomBone()
    {
        if (joints.Count < 3)
        {
            AddRandomJoint();
            TryAddRandomBone();
        } else
        {
            Joint jointA = joints[Utils.ProgramRandom.Next(joints.Count)];
            Joint jointB = joints[Utils.ProgramRandom.Next(joints.Count)];

            if(jointA.tryAddConnection(jointB))
            {
                bones.Add(Bone.RandomBone(jointA, jointB));
                return true;
            }

        }
        return false;
    }

    public bool TryAddRandomMuscle()
    {
        Console.WriteLine("Trying to add random muscle");

        Joint jointA = joints[Utils.ProgramRandom.Next(joints.Count)];
        Joint jointB = joints[Utils.ProgramRandom.Next(joints.Count)];

        if (jointA.tryAddConnection(jointB))
        {
            Console.WriteLine("Adding random muscle");
            muscles.Add(Muscle.AddRandomMuscle(jointA, jointB));
            return true;
        }

        
        return false;
    }
}
