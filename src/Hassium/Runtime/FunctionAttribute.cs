using System;

namespace Hassium.Runtime
{
    public class FunctionAttribute : Attribute
    {
        public string SourceRepresentation { get; private set; }

        public FunctionAttribute(string sourceRep)
        {
            SourceRepresentation = sourceRep;
        }
    }
}
