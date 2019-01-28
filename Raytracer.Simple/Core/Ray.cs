using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raytracer.Simple.Core
{
    class Ray
    {
        public Vector3 O;
        public Vector3 D;

        public Ray(Vector3 o, Vector3 d)
        {
            O = o;
            D = d;
        }

        public Vector3 PointAt(float t)
        {
            return O + t * D;
        }
    }
}
