using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raytracer.Simple.Lights
{
    class Light
    {
        public Vector3 O;

        public Light(Vector3 o)
        {
            O = o;
        }
    }
}
