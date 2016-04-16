using System;
using System.Collections.Generic;

using Hassium.Runtime.StandardLibrary.Types;

namespace Hassium.Runtime
{
    public class StackFrame
    {
        public class Frame
        {
            private Dictionary<double, HassiumObject> variables = new Dictionary<double, HassiumObject>();
            public void Add(double index, HassiumObject value)
            {
                variables.Add(index, value);
            }
            public bool ContainsVariable(double index)
            {
                foreach (double indice in variables.Keys)
                    if (indice == index)
                        return true;
                return false;
            }
            public void Modify(double index, HassiumObject value)
            {
                variables[index] = value;
            }
            public HassiumObject GetVariable(double index)
            {
                return variables[index];
            }
        }
        public Stack<Frame> Frames;
        public StackFrame()
        {
            Frames = new Stack<Frame>();
        }

        public void EnterFrame()
        {
            Frames.Push(new Frame());
        }
        public void PopFrame()
        {
            Frames.Pop();
        }
        public void Add(double index, HassiumObject value = null)
        {
            Frames.Peek().Add(index, value);
        }
        public bool Contains(double index)
        {
            foreach (Frame frame in Frames)
                if (frame.ContainsVariable(index))
                    return true;
            return false;
        }
        public void Modify(double index, HassiumObject value)
        {
            foreach (Frame frame in Frames)
            {
                if (frame.ContainsVariable(index))
                {
                    frame.Modify(index, value);
                    return;
                }
            }
        }
        public HassiumObject GetVariable(double index)
        {
            foreach (Frame frame in Frames)
                if (frame.ContainsVariable(index))
                    return frame.GetVariable(index);
            throw new Exception("Variable was not found inside the stack frame! Index " + index);
        }
    }
}