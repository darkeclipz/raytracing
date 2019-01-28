using Raytracer.Simple.Core;
using Raytracer.Simple.Materials;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raytracer.Simple.Shapes
{
    class Sphere : Shape
    {
        public float R;
        public float ZMin;
        public float ZMax;
        public float ThetaMin;
        public float ThetaMax;
        public float PhiMax;

        public Sphere(Vector3 o, Material m, float r) : base(o, m)
        {
            R = r;
            ZMin = -r;
            ZMax = r;
            ThetaMin = (float)Math.Acos(Math.Clamp(ZMin / r, -1f, 1f));
            ThetaMax = (float)Math.Acos(Math.Clamp(ZMax / r, -1f, 1f));
            PhiMax = 2 * (float)Math.PI;
        }

        public override Intersection Intersect(Ray r)
        {
            void Swap<T>(ref T a, ref T b)
            {
                T temp = a;
                a = b;
                b = temp;
            }

            // Determine if there is an intersection, and find out where.
            float A = Vector3.Dot(r.D, r.D);
            float B = 2f * Vector3.Dot(r.D, r.O - O);
            float C = Vector3.Dot(r.O - O, r.O - O) - R * R;
            float discr = B * B - 4f * A * C;
            if (discr < 0) return null; // negative discriminant, miss
            float rootDiscr = (float)Math.Sqrt(discr);
            float q;
            if (B < 0) q = -0.5f * (B - rootDiscr);
            else q = -0.5f * (B + rootDiscr);
            float t0 = q / A;
            float t1 = C / q;
            if (t0 > t1) Swap(ref t0, ref t1);
            if (t0 < 0) return null; // behind us, miss

            // We need the point of intersection (x,y,z), and
            // the normal of the geometry at the point of intersection.
            // We also add the shape itself to reference the material data.
            var result = new Intersection
            {
                O = r.PointAt(t0),
                Shape = this,
                Normal = Vector3.Normalize(r.PointAt(t0) - O),
                T = t0 // Distance from the camera to the point of intersection
                       // on the object.
            };

            // Calculate UV coordinates for texture mapping. Transforms spherical
            // coordinates to UV coordinates between [0,1]^2.
            var phit = r.PointAt(t0) - O;
            if (phit.X == 0f && phit.Y == 0f) phit.X = 1e-5f * R;
            float phi = (float)Math.Atan2(phit.Y, phit.X);
            if (phi < 0f) phi += 2f * (float)Math.PI;
            float u = phi / PhiMax;
            float theta = (float)Math.Acos(Math.Clamp(phit.Z / R, -1f, 1f));
            float v = (theta - ThetaMin) / (ThetaMax - ThetaMin);
            result.UV = new Vector2(u, v);

            return result;
        }

        public override bool IntersectP(Ray r)
        {
            // Only determine if there is an intersection, but
            // not the actual position.
            float A = Vector3.Dot(r.D, r.D);
            float B = 2f * Vector3.Dot(r.D, r.O - O);
            float C = Vector3.Dot(r.O - O, r.O - O) - R * R;
            float discr = B * B - 4f * A * C;
            if (discr < 0) return false;
            return true;
        }
    }
}
