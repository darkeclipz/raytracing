using Raytracer.Simple.Shapes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raytracer.Simple.Core
{
    class Intersection
    {
        public Vector3 O;
        public Vector3 Normal;
        public Vector2 UV;
        public Shape Shape;
        public float T;
    }
}
