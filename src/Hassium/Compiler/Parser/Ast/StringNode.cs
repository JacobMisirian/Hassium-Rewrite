namespace Hassium.Compiler.Parser.Ast
{
    public class StringNode : AstNode
    {
        public override SourceLocation SourceLocation { get; }

        public string Value { get; private set; }

        public StringNode(SourceLocation location, string value)
        {
            SourceLocation = location;

            Value = value;
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
