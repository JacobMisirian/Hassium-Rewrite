using System.Collections.Generic;

namespace Hassium.Compiler.Parser.Ast
{
    public class FunctionDeclarationNode : AstNode
    {
        public override SourceLocation SourceLocation { get; }

        public string Name { get; private set; }
        public List<FunctionParameter> Parameters { get; private set; }
        public AstNode Body { get; private set; }
        public AstNode EnforcedReturnType { get; private set; }

        public FunctionDeclarationNode(SourceLocation location, string name, List<FunctionParameter> parameters, AstNode body)
        {
            SourceLocation = location;

            Name = name;
            Parameters = parameters;
            Body = body;
            EnforcedReturnType = null;
        }
        public FunctionDeclarationNode(SourceLocation location, string name, List<FunctionParameter> parameters, AstNode enforcedReturnType, AstNode body)
        {
            SourceLocation = location;

            Name = name;
            Parameters = parameters;
            Body = body;
            EnforcedReturnType = enforcedReturnType;
        }

        public override void Visit(IVisitor visitor)
        {
            visitor.Accept(this);
        }
        public override void VisitChildren(IVisitor visitor)
        {
            Body.Visit(visitor);
            EnforcedReturnType.Visit(visitor);
        }
    }
}
