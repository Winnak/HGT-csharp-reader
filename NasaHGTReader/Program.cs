using System;
using System.Drawing;
using System.IO;

using CommandLine;

namespace NasaHGTReader
{
    public class Program
    {
        public const int size = 1201;
        public const string directory = "heightmap";

        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args);

            Console.WriteLine("Heightmap to image");
            Console.WriteLine("Input file:\t" + result.Value.InpuFilename);
            Console.WriteLine("Output file:\t" + result.Value.OutputFilename);
            Console.WriteLine("Dimensions:\t{0}x{0}", size);
            if (result.Value.Normalize)
            {
                Console.WriteLine("Data will be normalized.");
            }

            Console.WriteLine();

            var data = ConvertBytes(File.ReadAllBytes(result.Value.InpuFilename));
            Console.WriteLine("File read.");

            if (result.Value.Normalize)
            {
                short lowest = short.MaxValue;
                short heighest = -32767;

                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] < lowest)
                    {
                        if (data[i] != short.MinValue)
                        {
                            lowest = data[i];
                        }
                        else
                        {
                            data[i] = (short)(heighest / 2);
                        }
                    }
                    else if (data[i] > heighest)
                    {
                        heighest = data[i];
                    }
                }
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = (short)(((float)(data[i] - lowest) / (float)(heighest - lowest) * 2 - 1) * short.MaxValue);
                }
                Console.WriteLine("Data normalized");
            }

            Console.WriteLine("Creating " + result.Value.Format + "-file");
            CreatePng(data, result.Value.OutputFilename + ".png");
            Console.WriteLine("File " + result.Value.OutputFilename + " was successfully created");
        }

        private static short[] ConvertBytes(byte[] bytes)
        {
            short[] data = new short[bytes.Length / 2];

            for (int i = 0; i < bytes.Length; i += 2)
            {
                data[i / 2] = (short)(bytes[i] << 8 | bytes[i + 1]);
            }

            return data;
        }

        private static void CreatePng(short[] data, string outFileName)
        {
            Color[] grayscale = new Color[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                int value = (int)(data[i] / 256) + 128;
                grayscale[i] = Color.FromArgb(value, value, value);
            }

            Bitmap image = new Bitmap(size, size);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    image.SetPixel(x, y, grayscale[y * size + x]);
                }
            }

            image.Save(outFileName);
        }
    }
}
