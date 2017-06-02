using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hassium.Compiler
{
    public class SourceLocation
    {
        public string File { get; private set; }

        public int Row { get; private set; }
        public int Column { get; private set; }

        public SourceLocation(string file, int row, int column)
        {
            File = file;

            Row = row;
            Column = column;
        }
    }
}
