using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raytracer.Core
{
    public class Normal
    {
        public Vector3 D;

        public float X { get => D.X; }
        public float Y { get => D.Y; }
        public float Z { get => D.Z; }

        public Normal(float x, float y, float z)
            => D = new Vector3(x, y, z);

        public Normal(Vector3 direction)
            => D = direction;
    }
}
