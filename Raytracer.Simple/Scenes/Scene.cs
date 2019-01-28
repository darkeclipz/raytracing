using Raytracer.Simple.Cameras;
using Raytracer.Simple.Lights;
using Raytracer.Simple.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raytracer.Simple.Scenes
{
    abstract class Scene
    {
        public Camera Camera;
        public List<Shape> Shapes;
        public List<Light> Lights;

        public int screenWidth;
        public int screenHeight;
        protected int _shapeId;

        public Scene(int width, int height)
        {
            Shapes = new List<Shape>();
            Lights = new List<Light>();
            screenWidth = width;
            screenHeight = height;
        }
    }
}
