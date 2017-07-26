using System;
using System.IO;

using Hassium.Compiler.Emit;
using Hassium.Compiler.Exceptions;
using Hassium.Runtime;

namespace Hassium
{
    public class Program
    {
        static void Main(string[] args)
        {

            try
            {
                var module = HassiumCompiler.CompileModuleFromFilePath(args[0]);

                /*      foreach (var attribPair in module.Attributes["__global__"].Attributes)
                      {
                          Console.WriteLine("{0}:", attribPair.Key);
                          foreach (var instruction in (attribPair.Value as HassiumMethod).Instructions)
                              Console.WriteLine(instruction);
                      }*/

                new VirtualMachine().Execute(module, args);
            }
            catch (ParserException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex);
            }
        }
    }
}
