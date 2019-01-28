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

    class RefractionScene : Scene
    {
        public RefractionScene(int width, int height) : base(width, height)
        {
            Camera = new Camera(new Vector3(0, -2, -5), new Vector3(0, 0, 1), 1, screenWidth, screenHeight);
            AddLights();
            AddWorld();
            AddSpheres();
        }

        public void AddLights()
        {
            Lights.Add(new SphereLight(new Vector3(0, -20, -10), 10f, new Vector3(255), new Vector3(255), 4f, 10f));
        }

        public void AddWorld()
        {
            var bigSphereMaterial = new Material
            {
                Color = new Vector3(150),
                Texture = new CheckerPattern(300),
                TextureAlpha = 1f,
                Shade = false
            };

            var bigSphere = new Sphere(new Vector3(0, 100f, 0), bigSphereMaterial, 100)
            {
                ShapeId = _shapeId++
            };

            Shapes.Add(bigSphere);
        }

        public void AddSpheres()
        {
            var palette = new MagentaOrangePalette();

            var mat1 = new Material
            {
                Color = new Vector3(98, 0, 238),
                Texture = new CheckerPattern(20),
                TextureAlpha = 1f,
            };

            var mat2 = new Material
            {
                Color = new Vector3(3, 218, 198),
                Transmissiveness = 1f,
                RefractionIndex = 1f
            };

            float h1 = 100f - Vector3.Distance(new Vector3(-4, -2, 3), Shapes[0].O);
            Shapes.Add(new Sphere(new Vector3(0, -4, 6), mat1, 4));
            Shapes.Add(new Sphere(new Vector3(0, -2, 2), mat2, 2));
            
        }
    }
}
