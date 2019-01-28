using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raytracer.Core
{
    public class Transform
    {
        public Matrix4x4 M;
        public Matrix4x4 MInv;

        public Transform()
        {
            M = new Matrix4x4();
            MInv = new Matrix4x4();
        }

        public Transform(Matrix4x4 m)
        {
            M = m;
            Matrix4x4.Invert(m, out MInv);
        }

        public Transform(Matrix4x4 m, Matrix4x4 mInv)
        {
            M = m;
            MInv = mInv;
        }

        public static Transform Inverse(Transform t)
        {
            return new Transform(t.MInv, t.M);
        }

        public static Transform Translate(Vector3 v)
        {
            return new Transform
            {
                M = Matrix4x4.CreateTranslation(v),
                MInv = Matrix4x4.CreateTranslation(-v)
            };
        }

        public static Transform Scale(float scale)
        {
            return new Transform
            {
                M = Matrix4x4.CreateScale(scale),
                MInv = Matrix4x4.CreateScale(-scale)
            };
        }

        public static Transform operator *(Transform left, Transform right)
        {
            return new Transform(left.M * right.M, left.MInv * right.MInv);
        }

        public Point ApplyToPoint(Point p)
        {
            return new Point(Vector3.Zero);
        }

        public Vector3 ApplyToVector(Vector3 v)
        {
            return v;
        }

        public Ray ApplyToRay(Ray r)
        {
            return r;
        }

    }
}
