using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raytracer.Simple.Textures
{
    class GridPattern : Texture
    {
        private float _repetition;
        private float _width;
        private float _rotation;

        public GridPattern(float repetition, float width, float rotation = 0)
        {
            _repetition = repetition;
            _width = width;
            _rotation = rotation;
        }

        public override Vector4 GetUV(Vector2 uv)
        {
            //float cos = (float)Math.Cos(_rotation);
            //float sin = (float)Math.Sin(_rotation);
            //uv = new Vector2(cos * uv.X + sin * uv.Y, -sin * uv.Y + cos * uv.X);
            float uFract = (uv.X * _repetition) % 1f;
            float vFract = (uv.Y * _repetition) % 1f;
            return uFract < _width || vFract < _width
                ? new Vector4(0, 0, 0, 1)
                : new Vector4(1, 1, 1, 0);
        }
    }
}
