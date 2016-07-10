using System;
using OctopusProjectBuilder.YamlReader.Model;

namespace OctopusProjectBuilder.DocGen
{
    class Program
    {
        static void Main(string[] args)
        {
            var generator = new DocGenerator(typeof(YamlOctopusModel));
            Console.WriteLine(generator.Generate());
        }
    }
}
