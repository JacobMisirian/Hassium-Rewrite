using System;
using System.Collections.Generic;

using Hassium.Runtime.StandardLibrary.Types;

namespace Hassium
{
    public class SymbolTable
    {
        public class Scope
        {
            public Dictionary<string, int> symbols = new Dictionary<string, int>();
            public bool Contains(string symbol)
            {
                return symbols.ContainsKey(symbol);
            }

            public void Add(string symbol, int index)
            {
                symbols.Add(symbol, index);
            }

            public int GetIndex(string symbol)
            {
                return symbols[symbol];
            }
        }

        public Stack<Scope> Scopes { get; private set; }
        public Scope GlobalScope { get; private set; }
        public int CurrentIndex { get { return currentIndex; } set { currentIndex = value; } }
        private int currentIndex = 0;

        public SymbolTable()
        {
            Scopes = new Stack<Scope>();
            GlobalScope = new Scope();
            Scopes.Push(GlobalScope);
        }

        public void EnterScope()
        {
            Scopes.Push(new Scope());
        }

        public bool FindSymbol(string symbol)
        {
            foreach (Scope scope in Scopes)
                if (scope.Contains(symbol))
                    return true;
            return false;
        }

        public void PopScope()
        {
            Scopes.Pop();
            if (Scopes.Count <= 2)
                currentIndex = 0;
        }
        public void PopScope(string name)
        {
            Scopes.Pop();
            if (Scopes.Count <= 2)
            {
                GlobalScope.Add(name, currentIndex);
                currentIndex = 0;
            }
        }

        public void AddSymbol(string symbol)
        {
            Scopes.Peek().Add(symbol, currentIndex++);
        }

        public int GetIndex(string symbol)
        {
            foreach (Scope scope in Scopes)
                if (scope.Contains(symbol))
                    return scope.GetIndex(symbol);
            throw new Exception(string.Format("{0} does not exist in the current scope!", symbol));
        }

        public int GetGlobalIndex(string symbol)
        {
            return GlobalScope.GetIndex(symbol);
        }
    }
}