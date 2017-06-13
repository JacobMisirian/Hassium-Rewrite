using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hassium.Compiler.Emit;
using Hassium.Compiler.Exceptions;
using Hassium.Compiler.Lexer;
using Hassium.Compiler.Parser;
using Hassium.Runtime;
using Hassium.Runtime.Types;

namespace Hassium
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string code = Console.ReadLine();
                var tokens = new Scanner().Scan("stdin", code);

                var ast = new Parser().Parse(tokens);

                var module = new HassiumCompiler().Compile(ast);

                foreach (var attribPair in module.Attributes["__global__"].Attributes)
                {
                    Console.WriteLine("{0}:", attribPair.Key);
                    foreach (var instruction in (attribPair.Value as HassiumMethod).Instructions)
                        Console.WriteLine(instruction);
                }
            }
            catch (ParserException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex);
            }
        }
    }
}
