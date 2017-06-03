namespace Hassium.Compiler.Parser.Ast
{
    public class CodeBlockNode : AstNode
    {
        public override SourceLocation SourceLocation { get; }

        public CodeBlockNode(SourceLocation location)
        {
            SourceLocation = location;
        }

        public override void Visit(IVisitor visitor)
        {
            visitor.Accept(this);
        }
        public override void VisitChildren(IVisitor visitor)
        {

        }
    }
}
