﻿using System;
using System.Collections.Generic;

namespace Hassium.Compiler.Parser.Ast
{
    public class FuncNode: AstNode
    {
        public string Name { get; private set; }
        public List<FuncParameter> Parameters { get; private set; }
        public AstNode Body { get { return Children[0]; } }
        public string ReturnType { get; private set; }
        public bool EnforcesReturn { get { return ReturnType != string.Empty; } }

        public FuncNode(SourceLocation location, string name, List<FuncParameter> parameters, AstNode body, string returnType = "")
        {
            this.SourceLocation = location;
            Name = name;
            Parameters = parameters;
            Children.Add(body);
            ReturnType = returnType;
        }

        public override void Visit(IVisitor visitor)
        {
            visitor.Accept(this);
        }
        public override void VisitChildren(IVisitor visitor)
        {
            foreach (AstNode child in Children)
                child.Visit(visitor);
        }
    }

    public class FuncParameter
    {
        public bool IsEnforced { get { return Type != string.Empty; } }
        public string Name { get; private set; }
        public string Type { get; private set; }

        public FuncParameter(string name)
        {
            Name = name;
            Type = string.Empty;
        }
        public FuncParameter(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}

