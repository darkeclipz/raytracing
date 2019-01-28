#define SS4
using Raytracer.Simple.Core;
using Raytracer.Simple.Scenes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Numerics;

namespace Raytracer.Simple
{
    class Program
    {
        static void Main(string[] args)
        {
            int screenWidth = 1920/2;
            int screenHeight = 1080/2;
            bool useSS = false;
            int samples = 1;
#if SS4
            screenWidth = 1920;
            screenHeight = 1080;
            useSS = true;
            samples = 8;
#endif
            var scene = new SphereTowerScene(screenWidth, screenHeight);
            var renderer = new Renderer(scene);
            renderer.UseSupersampling = useSS;
            renderer.Samples = samples;

            Console.WriteLine($"Starting to render image with dimension: {screenWidth}x{screenHeight}");
            Console.WriteLine($"Supersampling: {(renderer.UseSupersampling ? "yes" : "no")}");
            Console.WriteLine($"Samples per pixel: {renderer.Samples}");
            var film = renderer.Render();

            Console.Write("\r");
            Console.WriteLine("Finished rendering!");
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), $"renderings/{DateTime.UtcNow.ToString("yyyyMMdd_HHmmss")}_{film.Name}.png");
            film.B.Save(filePath, ImageFormat.Png);
            Console.WriteLine($"Written image to {filePath}");
        }
    }

}
