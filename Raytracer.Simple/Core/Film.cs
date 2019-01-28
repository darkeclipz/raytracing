using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Raytracer.Simple.Core
{
    class Film
    {
        public Bitmap B;
        public string Name;
        public Film(int width, int height, string name)
        {
            Name = name;
            B = new Bitmap(width, height);
        }
    }
}
