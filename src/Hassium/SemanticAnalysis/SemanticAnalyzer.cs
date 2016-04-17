using System;

using Hassium.Parser;

namespace Hassium.SemanticAnalysis
{
    public class SemanticAnalyzer : IVisitor
    {
        private AstNode code;
        private SymbolTable result;

        public SymbolTable Analyze(AstNode ast)
        {
            code = ast;
            result = new SymbolTable();
            result.EnterScope();
            code.VisitChildren(this);
            return result;
        }

        public void Accept(ArgListNode node)
        {
            node.VisitChildren(this);
        }
        public void Accept(AttributeAccessNode node) {}
        public void Accept(BinaryOperationNode node)
        {
            node.VisitChildren(this);
        }
        public void Accept(ClassNode node)
        {
            node.VisitChildren(this);
        }
        public void Accept(CodeBlockNode node)
        {
            result.EnterScope();
            node.VisitChildren(this);
            result.PopScope();
        }
        public void Accept(FuncNode node)
        {
            node.VisitChildren(this);
        }
        public void Accept(UnaryOperationNode node) {}
        public void Accept(IdentifierNode node) {}
        public void Accept(NumberNode node) {}
        public void Accept(CharNode node) {}
        public void Accept(ConditionalNode node)
        {
            node.VisitChildren(this);
        }
        public void Accept(FunctionCallNode node) {}
        public void Accept(ExpressionNode node) {}
        public void Accept(StatementNode node) {}
        public void Accept(StringNode node) {}
        public void Accept(WhileNode node)
        {
            node.VisitChildren(this);
        }
    }
}

