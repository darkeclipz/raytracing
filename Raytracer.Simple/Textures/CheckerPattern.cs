using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raytracer.Simple.Textures
{
    class CheckerPattern : Texture
    {
        private float _alpha;

        public CheckerPattern(float alpha)
        {
            _alpha = alpha;
        }

        public override Vector4 GetUV(Vector2 uv)
        {
            return ((int)(uv.X * _alpha) + (int)(uv.Y * _alpha)) % 2 == 0
                ? new Vector4(1, 1, 1, 0)
                : new Vector4(0, 0, 0, 1);
        }
    }
}
