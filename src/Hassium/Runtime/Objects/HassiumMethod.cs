using System;
using System.Collections.Generic;

using Hassium.Compiler;
using Hassium.Compiler.CodeGen;
using Hassium.Compiler.Parser.Ast;

namespace Hassium.Runtime.Objects
{
    public class HassiumMethod: HassiumObject
    {
        public static new HassiumTypeDefinition TypeDefinition = new HassiumTypeDefinition("func");

        public string Name { get; set; }
        public HassiumClass Parent { get; set; }

        public List<Instruction> Instructions { get; private set; }
        public Dictionary<FuncParameter, int> Parameters { get; private set; }

        public Dictionary<int, int> Labels { get; private set; }
        public Stack<int> BreakLabels { get; private set; }
        public Stack<int> ContinueLabels { get; private set; }

        public string ReturnType { get; set; }

        public bool IsConstructor { get { return Name == "new"; } }

        public HassiumMethod()
        {
            AddType(TypeDefinition);

            Instructions = new List<Instruction>();
            Parameters = new Dictionary<FuncParameter, int>();

            Labels = new Dictionary<int, int>();
            BreakLabels = new Stack<int>();
            ContinueLabels = new Stack<int>();
        }

        public void Emit(SourceLocation location, InstructionType instructionType, int argument = 0)
        {
            Instructions.Add(new Instruction(location, instructionType, argument));
        }
        public void EmitLabel(SourceLocation locatiom, int label)
        {
           // Labels.Add(label, Instructions.Count);
            Instructions.Add(new Instruction(locatiom, InstructionType.Label, label));
        }
    }
}

