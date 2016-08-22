using System;
using System.IO;

using Hassium.Compiler;
using Hassium.Compiler.Parser;
using Hassium.Compiler.Parser.Ast;
using Hassium.Compiler.Scanner;
using Hassium.Compiler.SemanticAnalysis;
using Hassium.Compiler.CodeGen;
using Hassium.Runtime;

namespace Hassium
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var vm = new VirtualMachine();
            try
            {
                vm.Execute(Compiler.CodeGen.Compiler.CompileModuleFromSource(File.ReadAllText(args[0])), new System.Collections.Generic.List<string>());
            }
            catch (CompileException ex)
            {
                Console.WriteLine("At {0}:", ex.SourceLocation);
                Console.WriteLine(ex.Message);
            }
            catch (InternalException ex)
            {
                Console.WriteLine("At location {0}:", ex.VM.CurrentSourceLocation);
                Console.WriteLine("{0} at:", ex.Message);
                while (ex.VM.CallStack.Count > 0)
                    Console.WriteLine(ex.VM.CallStack.Pop());
            }
        }
    }
}