using System;

using Hassium.Parser;

namespace Hassium.CodeGen
{
    public class HassiumCompiler
    {
        public HassiumModule Compile(AstNode ast, SymbolTable table, string name)
        {
            return new ModuleCompiler().Compile(ast, table, name);
        }
    }
}

