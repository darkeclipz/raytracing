using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raytracer.Simple.Textures
{
    abstract class Texture
    {
        public abstract Vector4 GetUV(Vector2 uv);
    }
}
