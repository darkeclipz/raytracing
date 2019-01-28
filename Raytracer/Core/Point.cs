using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raytracer.Core
{
    public class Point
    {
        public Vector3 O;

        public float X { get => O.X; }
        public float Y { get => O.Y; }
        public float Z { get => O.Z; }

        public Point(Vector3 position)
            => O = position;

        public Point(float x, float y, float z)
            => O = new Vector3(x, y, z);

        public static Point operator +(Point left, Point right)
            => new Point(left.O + right.O);

        public static Point operator +(Vector3 left, Point right)
            => new Point(left + right.O);

        public static Point operator +(Point left, Vector3 right)
            => new Point(left.O + right);

        public static Point operator -(Point left, Point right)
            => new Point(left.O - right.O);

        public static Point operator -(Vector3 left, Point right)
            => new Point(left - right.O);

        public static Point operator -(Point left, Vector3 right)
            => new Point(left.O - right);

        public static float Distance(Point p1, Point p2)
            => (p2.O - p1.O).Length();

        public static float DistanceSquared(Point p1, Point p2)
            => (p2.O - p1.O).LengthSquared();
    }
}
