using System;
using Commander.HelloWorld.Configuration;

namespace Commander.HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            var graph = new HelloWorldCommandRegistry().BuildGraph();
            Console.ReadLine();
        }
    }
}
