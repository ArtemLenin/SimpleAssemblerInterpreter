using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Assambler
{
    class Lexer
    {
        readonly string s;
        public int pos;

        public Lexer(string _s)
        {
            s = _s;
        }

        #region rx

        static readonly string tokenRx = new RegexBuilder {
                {"word", @"[A-Za-z_][A-Za-z_0-9:]*" }, // {"word", @"[a-zA-Z]+(|:)"
                { "number", @"\d+" },
                { "operation", @"[-+*/<>=]+" },
                { "whiteSpace", @"\s+"},
            }.Build();

        const RegexOptions regexOptions = RegexOptions.Compiled | RegexOptions.CultureInvariant |RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.Singleline;

        static readonly Regex tokenRegex = new Regex(tokenRx, regexOptions);
        
        #endregion

        Exception Error(string format, params object[] args)
        {
            return new Exception(string.Format(format, args) + string.Format(", текущая позиция: {0}", pos));
        }

        public Token GetNextToken()
        {
            var match = tokenRegex.Match(s, pos);
            if (!match.Success)
            {
                throw Error("Ничего не найдено");
            }
            if (pos < match.Index)
            {
                throw Error("Пропущено: \"{0}\"", s.Substring(pos, match.Length));
            }
            if (match.Length == 0)
            {
                throw Error("Найден токен нулевой длины, неверно составлена регулярка");
            }
            pos += match.Length;
            var groups = match.Groups;
            var lexeme = match.Value;
            if (groups["operation"].Success)
            {
                return new Token(TokenType.operation, lexeme);
            }
            if (groups["number"].Success)
            {
                return new Token(TokenType.number, lexeme);
            }
            if (groups["word"].Success)
            {
                return new Token(TokenType.variable, lexeme);
            }
            if (groups["whiteSpace"].Success)
            {
                return new Token(TokenType.whitespase, lexeme);
            }
            throw Error("Нашли \"{0}\", но непонятно что это", lexeme);
        }

        public IEnumerable<Token> GetTokens()
        {
            while (pos < s.Length)
            {
                yield return GetNextToken();
            }
        }
    }
}


