using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Numerics;

namespace Raytracer
{
    class Program
    {
        static void Main(string[] args)
        {
            var image = new Image(800, 600);

            for(int i=0; i < image.Width; i++)
            {
                var color = new Vector3(i / image.Width) * 255;

                for (int j = 0; j < image.Height; j++)
                    image.SetPixel(new Vector2(i, j), color);
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "hello_world.png");
            image.Save(filePath);
            Console.WriteLine($"Image saved to {filePath}");
            Console.ReadKey();
        }
    }

    class Image
    {
        public int Width;
        public int Height;
        public byte[] Buffer;
        public int BufferSize;
        private readonly int _components = 4;

        public Image(int width, int height)
        {
            Width = width;
            Height = height;
            Initialize();
        }

        private void Initialize()
        {
            BufferSize = 256 * Width * Height * _components;
            Buffer = new byte[BufferSize];
        }

        public void SetPixel(Vector2 pos, Vector3 color)
        {
            var offset = (int)(_components * (256 * pos.Y + pos.X));
            Buffer[offset] = (byte)(int)color.Z;
            Buffer[offset + 1] = (byte)(int)color.Y;
            Buffer[offset + 2] = (byte)(int)color.X;
            Buffer[offset + 3] = 255;
        }
        
        public void Save(string filePath)
        {
            unsafe
            {
                fixed(byte* ptr = Buffer)
                {
                    using(var image = new Bitmap(Width, Height, 256 * _components, PixelFormat.Format32bppRgb, new IntPtr(ptr)))
                    {
                        image.Save(filePath);
                    }
                }
            }
        }
    }

}
