using System;
using System.IO;

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
            AstNode ast = null;
            try
            {
                var tokens = new Lexer().Scan(File.ReadAllText(args[0]));
                ast = new Parser().Parse(tokens);
                var table = new SemanticAnalyzer().Analyze(ast);
                var module = new Compiler.CodeGen.Compiler().Compile(ast, table);
                vm.Execute(module, new System.Collections.Generic.List<string>());
            }
            catch (ExpectedTokenException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (UnexpectedTokenException ex)
            {
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