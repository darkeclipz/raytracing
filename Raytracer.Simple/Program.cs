//#define SS4
using Raytracer.Simple.Core;
using Raytracer.Simple.Renderers;
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
            int samples = 4;
            int softShadowRays = 32;
#if SS4
            screenWidth = 1920*2;
            screenHeight = 1080*2;
            useSS = true;
            samples = 4;
            softShadowRays = 64;
#endif
            Scene scene = new SphereTowerMatteScene(screenWidth, screenHeight);
            var renderer = new BlinnPhongRenderer(scene);
            renderer.UseSupersampling = useSS;
            renderer.Samples = samples;
            renderer.SoftShadowRays = softShadowRays;

            if(samples > 1 && !useSS) renderer.Samples = 1;
            if (samples == 1) renderer.UseSupersampling = false;

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"Starting to render image with dimension: {screenWidth}x{screenHeight}");
            Console.WriteLine($"Supersampling: {(renderer.UseSupersampling ? "yes" : "no")}");
            Console.WriteLine($"Samples per pixel: {renderer.Samples}");
            Console.WriteLine($"Soft shadow rays: {renderer.SoftShadowRays}");
            var film = renderer.Render();

            Console.Write("\r");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Finished rendering!                            ");
            Console.ForegroundColor = ConsoleColor.Gray;
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), $"renderings/{DateTime.UtcNow.ToString("yyyyMMdd_HHmmss")}_{film.Name}.png");
            film.B.Save(filePath, ImageFormat.Png);
            Console.WriteLine($"Written image to {filePath}");
        }
    }

}
