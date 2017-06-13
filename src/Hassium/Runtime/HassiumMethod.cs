using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hassium.Compiler;
using Hassium.Compiler.Emit;
using Hassium.Compiler.Parser;

namespace Hassium.Runtime
{
    public class HassiumMethod : HassiumObject
    {
        public Stack<int> BreakLabels { get; private set; }
        public Stack<int> ContinueLabels { get; private set; }
        public Dictionary<int, int> Labels { get; private set; }

        public string Name { get; set; }
        public bool IsConstructor { get { return Name == "new"; } }

        public Dictionary<FunctionParameter, int> Parameters { get; private set; }
        public List<HassiumInstruction> Instructions { get; private set; }
        public HassiumMethod ReturnType { get; private set; }

        public HassiumMethod()
        {
            BreakLabels = new Stack<int>();
            ContinueLabels = new Stack<int>();
            Instructions = new List<HassiumInstruction>();
            Labels = new Dictionary<int, int>();
            Parameters = new Dictionary<FunctionParameter, int>();
        }
        public HassiumMethod(string name)
        {
            BreakLabels = new Stack<int>();
            ContinueLabels = new Stack<int>();
            Instructions = new List<HassiumInstruction>();
            Labels = new Dictionary<int, int>();
            Parameters = new Dictionary<FunctionParameter, int>();

            Name = name;
        }

        public void Emit(SourceLocation location, InstructionType instructionType, int argument = 0)
        {
            Instructions.Add(new HassiumInstruction(location, instructionType, argument));
        }
        public void EmitLabel(SourceLocation location, int label)
        {
            Labels.Add(label, Instructions.Count - 1);
        }

        public static Dictionary<TKey, TValue> CloneDictionary<TKey, TValue>
 (Dictionary<TKey, TValue> original) where TValue : ICloneable
        {
            Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>(original.Count,
                original.Comparer);
            foreach (KeyValuePair<TKey, TValue> entry in original)
            {
                ret.Add(entry.Key, (TValue)entry.Value.Clone());
            }
            return ret;
        }
    }
}
