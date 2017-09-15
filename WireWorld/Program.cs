﻿using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Keyboard");
            Console.WriteLine("  Tab: Force update (even if paused)");
            Console.WriteLine("  Space: Toggle pause");
            Console.WriteLine("  Up: Increase update speed");
            Console.WriteLine("  Down: Decrease update speed");
            Console.WriteLine();

            Console.WriteLine("Mouse");
            Console.WriteLine("  Left: Copper");
            Console.WriteLine("  Right: Void");
            Console.WriteLine("  Middle: Head");

            new Game().Run(60);
        }
    }
}