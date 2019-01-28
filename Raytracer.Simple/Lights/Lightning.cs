using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raytracer.Simple.Lights
{
    class Lightning
    {
        public Vector3 Diffuse;
        public Vector3 Specular;
        public float Lambertian;

        public Lightning() { }
        public Lightning(Vector3 diff, Vector3 spec, float lambertian)
        {
            Diffuse = diff;
            Specular = spec;
            Lambertian = lambertian;
        }
    }
}
