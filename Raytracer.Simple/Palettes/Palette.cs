using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raytracer.Simple.Palettes
{
    abstract class Palette
    {
        public Vector3 A;
        public Vector3 B;
        public Vector3 C;
        public Vector3 D;

        public Vector3 GetColor(float t)
        {
            float r = A.X + B.X * (float)Math.Cos(2 * Math.PI * (C.X * t + D.X));
            float g = A.Y + B.Y * (float)Math.Cos(2 * Math.PI * (C.Y * t + D.Y));
            float b = A.Z + B.Z * (float)Math.Cos(2 * Math.PI * (C.Z * t + D.Z));
            return new Vector3(r, g, b);
        }
    }
}
