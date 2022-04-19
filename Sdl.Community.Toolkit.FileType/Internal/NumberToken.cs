using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdl.Community.Toolkit.FileType.Internal
{
    internal class NumberToken
    {
        public enum TokenType
        {
            Number,
            ThousandSeparator,
            DecimalSeparator,
            Separator,
            Invalid
        }

        public TokenType type { get; set; }
        public char value { get; set; }
    }
}
