using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SchemeInterpreter.LexerEngine
{
    public static class LexerGenerator
    {
        public static void Generate(String filename)
        {
            if (!filename.Contains(".miniflex"))
                throw new FileLoadException("File not supported");

            var text = File.ReadAllText(filename);
            var lexContent = text.Split(';');



            foreach (var line in lexContent)
            {

            }

        }
    }

    class ProgramTester
    {
        static void main(string[] args)
        {
            File.WriteAllText("test.miniflex", @"name: 'this is test';");
            LexerGenerator.Generate("test.miniflex");
            Console.ReadLine();
        }
    }
}
