using Raytracer.Simple.Core;
using Raytracer.Simple.Scenes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raytracer.Simple.Renderers
{
    class SimpleRenderer : Renderer
    {
        public SimpleRenderer(Scene scene) : base(scene) { }

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
                    float n1 = Vector3.Dot(r.D, cs.Normal) <= 0 ? 1f : cs.Shape.Material.RefractionIndex;
                    float n2 = Vector3.Dot(r.D, cs.Normal) <= 0 ? cs.Shape.Material.RefractionIndex : 1f;
                    var tRay = new Ray(cs.O, Refract(r.D, cs.Normal, n1, n2));
                    var tColor = Trace(tRay, scene, ++depth);
                    if (tColor != Vector3.Zero && float.TryParse(tColor.X.ToString(), out float _))
                        color = Vector3.Lerp(color, tColor, cs.Shape.Material.Transmissiveness);
                }

                //// Shading
                //var light = scene.Lights[0];
                //var lightning = light.GetLight(cs.O, r.D, cs.Normal);

                //if(lightning.Lambertian > 0f)
                //{
                //    color += lightning.Diffuse + lightning.Specular;
                //}

                // If the intersection point is in the shadows, add some black to it.
                if (TraceShadow(cs.O + cs.Normal * 0.001f, scene))
                {
                    color = Vector3.Lerp(color, Vector3.Zero, 0.5f);
                }

                // Add fog to intersections that are far away.
                color = Vector3.Lerp(color, Vector3.Zero, Math.Clamp(1f - 10f / cs.T, 0f, 1f));

                //// Gamma correction
                //float gamma = 1.2f;
                //color = new Vector3(
                //    (float)Math.Pow(color.X, 1.0f / gamma), 
                //    (float)Math.Pow(color.Y, 1.0f / gamma), 
                //    (float)Math.Pow(color.Z, 1.0f / gamma));
            }

            return color;
        }

        static bool TraceShadow(Vector3 o, Scene scene)
        {
            foreach (var light in scene.Lights)
            {
                var ray = new Ray(o, Vector3.Normalize(light.O - o));
                foreach (var shape in scene.Shapes)
                {
                    if (shape.Intersect(ray) != null) return true;
                }
            }
            return false;
        }

        static Vector3 Refract(Vector3 normal, Vector3 incident, float n1, float n2)
        {
            float n = n1 / n2;
            float cosI = -Vector3.Dot(normal, incident);
            float sinT2 = n * n * (1.0f - cosI * cosI);
            if (sinT2 > 1.0) return Vector3.Zero;
            float cosT = (float)Math.Sqrt(1.0 - sinT2);
            return n * incident + (n * cosI - cosT) * normal;
        }

    }
}
