using Raytracer.Simple.Core;
using Raytracer.Simple.Lights;
using Raytracer.Simple.Scenes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raytracer.Simple.Renderers
{
    class BlinnPhongRenderer : TaskRenderer
    {
        public BlinnPhongRenderer(Scene scene) : base(scene) { }

        public override Vector3 Trace(Ray r, Scene scene, int depth = 0)
        {
            if (depth > 32) { return Vector3.Zero; }

            // Find the closest shape.
            Intersection cs = null;
            foreach (var shape in scene.Shapes)
            {
                var hit = shape.Intersect(r);
                if (hit != null && (cs == null || hit.T < cs.T))
                {
                    cs = hit;
                }
            }

            // Set the background color.
            var color = Vector3.Lerp(new Vector3(83, 115, 155), new Vector3(140, 186, 222), r.D.Y + 0.5f);

            // If there is an intersection...
            if (cs != null)
            {
                // Get the color from the material
                color = cs.Shape.Material.Color;

                // If there is a texture, mix it with the texture alpha
                if (cs.Shape.Material.Texture != null)
                {
                    var rgba = cs.Shape.Material.Texture.GetUV(cs.UV);
                    var rgb = new Vector3(rgba.X, rgba.Y, rgba.Z) * 255;
                    var alpha = rgba.W;
                    var tColor = Vector3.Lerp(color, rgb, alpha);
                    color = Vector3.Lerp(color, tColor, cs.Shape.Material.TextureAlpha);
                }

                // Reflection
                if (cs.Shape.Material.Reflectivity > 0)
                {
                    var rRay = new Ray(cs.O, Vector3.Reflect(r.D, cs.Normal));
                    var rColor = Trace(rRay, scene, ++depth);
                    color = Vector3.Lerp(color, rColor, cs.Shape.Material.Reflectivity);
                }

                // Transmission
                if (cs.Shape.Material.Transmissiveness > 0)
                {
                    Vector3 rDir = Refract(r.D, cs.Normal, cs.Shape.Material.RefractionIndex);
                    bool outside = Vector3.Dot(r.D, cs.Normal) < 0;


                    Vector3 bias = rDir * 1e-5f;
                    if (!outside) bias = -bias;
                    var tRay = new Ray(cs.O + bias, rDir);

                    var tColor = Trace(tRay, scene, ++depth);
                    if (tColor != Vector3.Zero && float.TryParse(tColor.X.ToString(), out float _))
                        color = Vector3.Lerp(color, tColor, cs.Shape.Material.Transmissiveness);
                    
                    else {
                        //Console.WriteLine("TIR");
                    }
                }

                // Shading
                if(cs.Shape.Material.Shade)
                {
                    var light = scene.Lights[0];
                    var lightning = light.GetLight(cs.O, r.D, cs.Normal);

                    if (lightning.Lambertian > 0f)
                    {
                        color += lightning.Specular + lightning.Diffuse * (1f - cs.Shape.Material.Reflectivity);
                    }
                }

                // Soft shadows.
                color = Vector3.Lerp(color, Vector3.Zero, Math.Clamp(TraceSoftShadow(cs.O + cs.Normal * 1e-5f, scene), 0f, 0.75f));

                // Add fog to intersections that are far away.
                color = Vector3.Lerp(color, Vector3.Zero, Math.Clamp(1f - 10f / cs.T, 0f, 1f));
            }

            return color;
        }

        float TraceHardShadow(Vector3 o, Scene scene)
        {
            foreach (var light in scene.Lights)
            {
                foreach (var shape in scene.Shapes)
                {
                    var ray = new Ray(o, Vector3.Normalize(light.O - o));
                    if (shape.IntersectP(ray))
                    {
                        return 1f;
                    }
                }
            }

            return 0f;
        }

        float TraceSoftShadow(Vector3 o, Scene scene)
        {
            int hit = 0, miss = 0;

            foreach (var light in scene.Lights)
            {
                for (int i = 0; i < SoftShadowRays; i++)
                {
                    var dir = Vector3.Normalize(light.GetRandomSurfacePoint() - o);
                    var ray = new Ray(o, dir);

                    foreach (var shape in scene.Shapes)
                    {
                        if (shape.IntersectP(ray))
                        {
                            hit++;
                            miss--;
                            break;
                        }
                    }
                    miss++;
                    if (light is PointLight) continue;
                }
            }

            return (float)hit / (hit + miss);
        }

        Vector3? Refract(Vector3 incident, Vector3 normal, float n1, float n2)
        {
            float n = n1 / n2;
            float cosI = -Vector3.Dot(normal, incident);
            float sinT2 = n * n * (1.0f - cosI * cosI);
            if (sinT2 >= 1.0) return null;
            float cosT = (float)Math.Sqrt(1.0 - sinT2);
            return Vector3.Normalize(n * incident + (n * cosI - cosT) * normal);
        }

        Vector3 Refract(Vector3 incident, Vector3 normal, float ior)
        {
            void Swap<T>(ref T a, ref T b)
            {
                T temp = a;
                a = b;
                b = temp;
            }

            float cosi = Math.Clamp(Vector3.Dot(incident, normal), -1f, 1f);
            float etai = 1, etat = ior;
            Vector3 n = new Vector3(normal.X, normal.Y, normal.Z);
            if(cosi < 0) { cosi = -cosi; } else { Swap(ref etai, ref etat); n = -normal; }
            float eta = etai / etat;
            float k = 1 - eta * eta * (1 - cosi * cosi);
            return k < 0 ? Vector3.Zero : eta * incident + Vector3.One * (float)(eta * cosi - Math.Sqrt(k)) * n;
        }

    }
}
