using System;

using Hassium.Compiler.Parser;
using Hassium.Compiler.Parser.Ast;

namespace Hassium.Compiler.SemanticAnalysis
{
    public class SemanticAnalyzer : IVisitor
    {
        private AstNode code;
        private SymbolTable result;

        public SymbolTable Analyze(AstNode ast)
        {
            code = ast;
            result = new SymbolTable();
            result.PushScope();
            code.VisitChildren(this);
            return result;
        }

        public void Accept(ArgumentListNode node) {}
        public void Accept(AttributeAccessNode node) {}
        public void Accept(BinaryOperationNode node) {}
        public void Accept(CharNode node) {}
        public void Accept(ClassNode node) 
        {
            node.VisitChildren(this);
        }
        public void Accept(CodeBlockNode node)
        {
            result.PushScope();
            node.VisitChildren(this);
            result.PopScope();
        }
        public void Accept(ExpressionStatementNode node)
        {
            node.VisitChildren(this);
        }
        public void Accept(FloatNode node) {}
        public void Accept(FuncNode node)
        {
            node.VisitChildren(this);
        }
        public void Accept(FunctionCallNode node) {}
        public void Accept(IdentifierNode node) {}
        public void Accept(IfNode node) {}
        public void Accept(IntegerNode node) {}
        public void Accept(ListAccessNode node) {}
        public void Accept(ListDeclarationNode node) {}
        public void Accept(StatementNode node) {}
        public void Accept(StringNode node) {}
        public void Accept(UnaryOperationNode node) {}
        public void Accept(WhileNode node) {}
    }
}

