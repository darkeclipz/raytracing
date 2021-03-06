﻿using Raytracer.Simple.Cameras;
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

    class SphereTowerScene : Scene
    {
        public SphereTowerScene(int width, int height) : base(width, height)
        {
            Camera = new Camera(new Vector3(0, -2, -5), new Vector3(0, 0, 1), 1, screenWidth, screenHeight);
            AddLights();
            AddWorld();
            AddSpheres();
        }

        public void AddLights()
        {
            Lights.Add(new PointLight(new Vector3(0, -8, -2), 5f, new Vector3(255), new Vector3(255), 4f));
        }

        public void AddWorld()
        {
            var bigSphereMaterial = new Material
            {
                Color = new Vector3(150),
                Texture = new CheckerPattern(300),
                TextureAlpha = .8f,
                Shade = false
            };

            var bigSphere = new Sphere(new Vector3(0, 100.5f, 0), bigSphereMaterial, 100)
            {
                ShapeId = _shapeId++
            };

            Shapes.Add(bigSphere);
        }

        public void AddSpheres()
        {
            var palette = new MagentaOrangePalette();
            var mirrorMaterial = new Material
            {
                Color = new Vector3(210),
                Reflectivity = 1f
            };
            var checkerMaterial = new Material
            {
                Color = new Vector3(150),
                Texture = new GridPattern(20, 0.1f, (float)Math.PI/4),
                TextureAlpha = 0.8f
            };

            float offsetY = -1.1f;
            for(int i=0; i < 5; i++)
            {
                var sphere = new Sphere(new Vector3(0, 0, 2), mirrorMaterial, (6-(float)i)/6);
                sphere.R *= 0.8f;
                sphere.ShapeId = _shapeId++;
                offsetY += 2*sphere.R;
                sphere.O.Y = -offsetY;
                Shapes.Add(sphere);
            }

            float h1 = 100f - Vector3.Distance(new Vector3(-4, -2, 3), Shapes[0].O);
            Shapes.Add(new Sphere(new Vector3(-4, -1.5f, 3), mirrorMaterial, 2));
            Shapes.Add(new Sphere(new Vector3(4, -1.5f, 3), mirrorMaterial, 2));

            float h2 = 100f - Vector3.Distance(new Vector3(-4, -2, 8), Shapes[0].O);
            Shapes.Add(new Sphere(new Vector3(-3, -1.1f, 8), mirrorMaterial, 2));
            Shapes.Add(new Sphere(new Vector3(3, -1.1f, 8), mirrorMaterial, 2));
        }
    }
}
