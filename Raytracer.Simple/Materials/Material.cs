using Raytracer.Simple.Textures;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raytracer.Simple.Materials
{
    class Material
    {
        public Vector3 Color;
        public float Reflectivity;
        public float Transmissiveness;
        public float RefractionIndex;
        public Texture Texture;
        public float TextureAlpha;
        public bool Shade = true;

        public Material() { }
        public Material(Vector3 color, float reflectivity = 0f, float transmissiveness = 0f, float refractionIndex = 1f, Texture texture = null, float textureAlpha = 0)
        {
            Color = color;
            Reflectivity = reflectivity;
            Transmissiveness = transmissiveness;
            RefractionIndex = refractionIndex;
            Texture = texture;
            TextureAlpha = textureAlpha;
        }
    }
}
