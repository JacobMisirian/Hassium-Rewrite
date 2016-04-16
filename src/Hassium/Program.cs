using System;
using System.IO;

using Hassium.CodeGen;
using Hassium.Lexer;
using Hassium.Parser;
using Hassium.SemanticAnalysis;
using Hassium.Runtime;

namespace Hassium
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Lexer.Lexer lexer = new Lexer.Lexer();
            Parser.Parser parser = new Parser.Parser();
            SemanticAnalyzer analyzer = new SemanticAnalyzer();
            HassiumCompiler compiler = new HassiumCompiler();
            VirtualMachine vm = new VirtualMachine();
            var tokens = lexer.Scan(File.ReadAllText(args[0]));
            var ast = parser.Parse(tokens);
            var table = analyzer.Analyze(ast);
            var module = compiler.Compile(ast, table, args[0]);
            vm.Execute(module);
        }

        private static int indentAst = 0;

        private static void writeAst(AstNode ast)
        {
            foreach (AstNode child in ast.Children)
            {
                for (int i = 0; i < indentAst; i++)
                    Console.Write("\t");
                Console.Write(child);
                Console.WriteLine();

                if (child.Children.Count > 0)
                {
                    indentAst++;
                    writeAst(child);
                }
                else
                    indentAst--;
            }
        }
    }
}
