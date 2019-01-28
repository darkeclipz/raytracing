using Raytracer.Simple.Core;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raytracer.Simple.Cameras
{
    class Camera
    {
        public Vector3 O;
        public Vector3 D;
        public float f;
        public int screenWidth;
        public int screenHeight;
        public float aspect;

        public Camera(Vector3 o, Vector3 d, float f, int w, int h)
        {
            O = o;
            D = d;
            this.f = f;
            screenWidth = w;
            screenHeight = h;
            aspect = (float)screenWidth / screenHeight;
        }

        public Ray GetRay(int x, int y, bool noOffset = false)
        {
            var rng = new Random();
            float offsetX = StaticRandom.Next();
            float offsetY = StaticRandom.Next();
            if (noOffset) { offsetX = 0; offsetY = 0; }
            var P = O + f * D + new Vector3((x + offsetX) / screenWidth, (y + offsetY) / screenHeight, 0) - new Vector3(.5f, .5f, 0f);
            P.X *= aspect;
            return new Ray(O, P - O);
        }
    }
}
