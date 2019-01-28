using Raytracer.Simple.Cameras;
using Raytracer.Simple.Lights;
using Raytracer.Simple.Materials;
using Raytracer.Simple.Palettes;
using Raytracer.Simple.Shapes;
using Raytracer.Simple.Textures;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raytracer.Simple.Scenes
{

    class SphereArrayScene : Scene
    {
        public SphereArrayScene(int width, int height) : base(width, height)
        {
            Camera = new Camera(new Vector3(0, -1, -18), new Vector3(0, 0, 1), 1, screenWidth, screenHeight);
            AddLights();
            AddWorld();
            AddSpheres();
        }

        public void AddLights()
        {
            Lights.Add(new PointLight(new Vector3(0, -10, -2), 5f, new Vector3(255), new Vector3(255), 4f));
        }

        public void AddWorld()
        {
            var bigSphereMaterial = new Material
            {
                Color = new Vector3(150),
                Texture = new GridPattern(1000, .15f, (float)Math.PI / 4),
                TextureAlpha = .125f
            };

            var bigSphere = new Sphere(new Vector3(0, 100.5f, 0), bigSphereMaterial, 100)
            {
                ShapeId = _shapeId++
            };

            Shapes.Add(bigSphere);
        }

        public void AddSpheres()
        {
            int gridX = 12;
            int gridZ = 12;
            float spacing = 4f;
            var palette = new MagentaOrangePalette();
            var mirrorMaterial = new Material
            {
                Color = new Vector3(210),
                Reflectivity = 1f,
                Texture = new CheckerPattern(10),
                TextureAlpha = .25f,
            };

            for (int i = 0; i <= gridX; i++)
            {
                for (int j = 0; j <= gridZ; j++)
                {
                    var coloredMaterial = new Material
                    {
                        Color = palette.GetColor((2 * ((float)(i * j) / (gridX * gridZ)) + 0.5f) % 1f) * 160,
                        Texture = new CheckerPattern(10),
                        TextureAlpha = 0.25f
                    };

                    var sphere = new Sphere(
                        new Vector3(spacing * i - gridX * spacing / 2, 0, spacing * j - gridZ * spacing / 2),
                        (i + j) % 2 == 0 ? mirrorMaterial : coloredMaterial,
                        1f)
                    {
                        ShapeId = _shapeId++
                    };

                    float dist = Vector3.Distance(sphere.O, Shapes[0].O);
                    sphere.O = new Vector3(sphere.O.X, dist - 100f - 1f, sphere.O.Z);
                    Shapes.Add(sphere);
                }
            }
        }
    }
}
