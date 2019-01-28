using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raytracer.Core
{
    public class Ray
    {
        public Point O;
        public Vector3 D;
        public float MinT;
        public float MaxT;
        public float Time;
        public int Depth;

    }
}
