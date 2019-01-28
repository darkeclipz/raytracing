using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Raytracer.Simple.Palettes
{
    class MagentaOrangePalette : Palette
    {
        public MagentaOrangePalette()
        {
            A = new Vector3(0.938f, 0.328f, 0.718f);
            B = new Vector3(0.659f, 0.438f, 0.328f);
            C = new Vector3(0.388f, 0.388f, 0.296f);
            D = new Vector3(2.538f, 2.478f, 0.168f);
        }
    }
}
