using Raytracer.Simple.Core;
using Raytracer.Simple.Scenes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raytracer.Simple.Renderers
{
    abstract class TaskRenderer
    {
        public Scene Scene;
        public bool UseSupersampling;
        public int Samples = 4;
        public int SoftShadowRays = 4;

        public TaskRenderer(Scene scene)
        {
            Scene = scene;
        }

        public Film Render()
        {
            var film = new Film(Scene.screenWidth, Scene.screenHeight, $"{Scene.screenWidth}x{Scene.screenHeight}_{Samples}X{(UseSupersampling ? "SS" : "")}");
            const int CORES = 8;
            const int THREAD_MULTIPLIER = 32;

            int taskPixelRange = Scene.screenWidth / (CORES * THREAD_MULTIPLIER);

            var tasks = new List<Task>();
            var monitorTask = MonitorTask(tasks, CORES * THREAD_MULTIPLIER);
            tasks.Add(monitorTask);
            for (int i = 0; i < CORES * THREAD_MULTIPLIER; i++)
            {
                var t = CreateTask(i * taskPixelRange, (i + 1) * taskPixelRange, film, Scene);
                t.Start();
                tasks.Add(t);
                if(i == 3) monitorTask.Start();
            }

            var lastTask = CreateTask(CORES * THREAD_MULTIPLIER * taskPixelRange, Scene.screenWidth, film, Scene);
            lastTask.Start();
            tasks.Add(lastTask);
            Task.WaitAll(tasks.ToArray());

            return film;
        }

        private Task CreateTask(int xStart, int xEnd, Film film, Scene scene)
        {
            var t = new Task(new Action(() =>
            {
                //Console.WriteLine($"Task started for x=[{xStart}, {xEnd}].");
                for (int x = xStart; x < xEnd; x++)
                {
                    for (int y = 0; y < scene.screenHeight; y++)
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
                        film.SetPixelSafe(x, y, Color.FromArgb(red, green, blue));
                    }
                }

            }));

            return t;
        }

        private Task MonitorTask(List<Task> tasks, int totalTasks)
        {
            return new Task(new Action(() =>
            {
                while(tasks.Count(t => !t.IsCompleted) > 1)
                {
                    Console.Write($"\rBlocks completed {tasks.Count(t => t.IsCompleted)}/{totalTasks}.           ");
                    Thread.Sleep(500);
                }
            }));
        }

        public abstract Vector3 Trace(Ray r, Scene scene, int depth = 0);
    }
}
