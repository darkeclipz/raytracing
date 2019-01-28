using Raytracer.Simple.Core;
using Raytracer.Simple.Scenes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Raytracer.Simple.Renderers
{
    abstract class Renderer
    {
        public Scene Scene;
        public bool UseSupersampling;
        public int Samples = 4;
        public int SoftShadowRays = 4;

        public Renderer(Scene scene)
        {
            Scene = scene;
        }

        public Film Render()
        {
            var film = new Film(Scene.screenWidth, Scene.screenHeight, $"{Scene.screenWidth}x{Scene.screenHeight}_{Samples}X{(UseSupersampling ? "SS" : "")}");

            for (int x = 0; x < Scene.screenWidth; x++)
            {
                if (x % 10 == 0) Console.Write($"\rProgress: {Math.Round((float)x / Scene.screenWidth * 100, 0)}%");

                for (int y = 0; y < Scene.screenHeight; y++)
                {
                    var col = Vector3.Zero;

                    for (int pass = 0; pass < (UseSupersampling ? Samples : 1); pass++)
                    {
                        var r = Scene.Camera.GetRay(x, y, noOffset: !UseSupersampling);
                        col += Trace(r, Scene);
                    }

                    col /= (UseSupersampling ? Samples : 1);
                    int red = (int)Math.Clamp(col.X, 0, 255);
                    int green = (int)Math.Clamp(col.Y, 0, 255);
                    int blue = (int)Math.Clamp(col.Z, 0, 255);
                    film.B.SetPixel(x, y, Color.FromArgb(red, green, blue));
                }
            }

            return film;
        }

        public abstract Vector3 Trace(Ray r, Scene scene, int depth = 0);
    }
}
