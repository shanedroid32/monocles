using System;
using Monocle;

namespace Example
{
    public static class Program
    {
        public static void Main()
        {
            using (var game = new ExampleGame())
                game.Run();
        }
    }
}