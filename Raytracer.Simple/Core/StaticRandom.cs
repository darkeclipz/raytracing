using System;
using System.Collections.Generic;
using System.Text;

namespace Raytracer.Simple.Core
{
    public static class StaticRandom
    {
        private static Random RNG;
        private static object @lock = false;

        public static float Next()
        {
            lock(@lock)
            {
                if (RNG == null) RNG = new Random();
                return (float)RNG.NextDouble();
            }
        }
    }
}
