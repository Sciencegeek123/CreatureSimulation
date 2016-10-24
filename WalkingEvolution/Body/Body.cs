using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Body
{
    private int joints;

    public Body(int j)
    {
        joints = j;
    }

    public bool AddBone(int A, int B, int optimalLength)
    {
        return true;
    }

    public bool AddMuscle(int A, int B)
    {
        return true;
    }
}
