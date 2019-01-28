using Raytracer.Simple.Core;
using Raytracer.Simple.Materials;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raytracer.Simple.Shapes
{
    abstract class Shape
    {
        public Vector3 O;
        public int ShapeId;
        public Material Material;

        public Shape(Vector3 o, Material m)
        {
            O = o;
            Material = m;
        }

        public abstract Intersection Intersect(Ray r);
        public abstract bool IntersectP(Ray r);
    }
}
