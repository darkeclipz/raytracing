using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raytracer.Simple.Lights
{
    class PointLight : Light
    {
        public PointLight(Vector3 position, float lightPower, Vector3 diffuseColor, Vector3 specularColor, float specularHardness)
        : base(position, lightPower, diffuseColor, specularColor, specularHardness) { }

        public override Lightning GetLight(Vector3 pos, Vector3 viewDir, Vector3 normal)
        {
            var lightning = new Lightning();
            var lightDir = O - pos;
            float distance = Vector3.Distance(O, pos);
            lightDir = Vector3.Normalize(lightDir);

            // Lambertian
            lightning.Lambertian = Math.Max(Vector3.Dot(lightDir, normal), 0f);
            
            // Intensity of the diffuse light
            float NdotL = Vector3.Dot(normal, lightDir);
            float intensity = Math.Clamp(NdotL, 0f, 1f);

            // Calculate the diffuse light factoring in light color, power and the attenuation.
            lightning.Diffuse = intensity * DiffuseColor * LightPower / distance;

            // Calculate the half vector between the light vector and the view vector.
            Vector3 H = Vector3.Normalize(lightDir + viewDir);

            // Intensity of specular light
            float NdotH = Vector3.Dot(normal, H);
            intensity = (float)Math.Pow(Math.Clamp(NdotH, 0f, 1f), SpecularHardness);

            // Sum up the specular light factoring
            lightning.Specular = intensity * SpecularColor * LightPower / distance;

            return lightning;
        }

        public override Vector3 GetRandomSurfacePoint()
        {
            return O;
        }
    }
}
