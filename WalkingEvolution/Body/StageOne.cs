using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class RandomBodyBuilder
{
    public static Body BuildStageOne(Random generator)
    {
        int joints = generator.Next(3, 32);

        Body myBody = new Body(joints);

        //First lets connect all the joints to the body.
        for(int i = 0; i < joints; i++)
        {
            int j, l;
            do
            {
                j = generator.Next(0, joints);
                if (j == i)
                {
                    j = (j + 1) % joints;
                }
                l = generator.Next(1, 32);
            } while(!myBody.AddBone(i, j, l));
        }

        //Do for some more bones
        int extraBones = generator.Next(0, joints * 2);

        for (int i = 0; i < joints; i++)
        {
            int j, l;
            do
            {
                j = generator.Next(0, joints);
                if (j == i)
                {
                    j = (j + 1) % joints;
                }
                l = generator.Next(1, 256);
            } while (!myBody.AddBone(i, j, l));
        }



        return myBody;

    }
}
