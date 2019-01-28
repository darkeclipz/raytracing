using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raytracer.Simple.Lights
{
    abstract class Light
    {
        public Vector3 O;
        public Vector3 DiffuseColor;
        public float LightPower;
        public Vector3 SpecularColor;
        public float SpecularHardness;

        public Light(Vector3 position, float lightPower, Vector3 diffuseColor, Vector3 specularColor, float specularHardness)
        {
            O = position;
            LightPower = lightPower;
            DiffuseColor = diffuseColor;
            SpecularColor = specularColor;
            SpecularHardness = specularHardness;
        }

        public abstract Lightning GetLight(Vector3 pos, Vector3 viewDir, Vector3 normal);
        public abstract Vector3 GetRandomSurfacePoint();
    }
}
