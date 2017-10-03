using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assambler
{
    enum TokenType
    {
        whitespase,
        variable,
        operation,
        number,
        eof
    }

    class Token
    {
        public readonly TokenType type;
        public readonly string lexeme;

        public Token(TokenType _type, string _lexeme)
        {
            type = _type;
            lexeme = _lexeme;
        }

        public override string ToString()
        {
            return string.Format("{0} \"{1}\"", type, lexeme);
        }
    }
}
