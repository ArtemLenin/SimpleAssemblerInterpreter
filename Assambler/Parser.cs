using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assambler
{
    class Parser : IDisposable
    {
        Dictionary<string, int> labels = new Dictionary<string, int>();

        readonly IEnumerator<Token> tokenEnumerator;
        Token currentToken;

        List<Node> code = new List<Node>();

        public Parser(IEnumerable<Token> tokens)
        {
            tokenEnumerator = tokens.GetEnumerator();
            Skip();
        }
        
        Exception Error(string format, params object[] args)
        {
            return new Exception(string.Format(format, args) + string.Format(", текущий токен: {0}", currentToken));
        }

        void Skip()
        {
            if (tokenEnumerator.MoveNext())
            {
                currentToken = tokenEnumerator.Current;
            }
            else
            {
                tokenEnumerator.Dispose();
                currentToken = new Token(TokenType.eof, "");
            }
        }

        void Skip(string s)
        {
            if (currentToken.lexeme.ToUpperInvariant() != s.ToUpperInvariant())
                throw Error($"ожидали {s}");
            Skip();
        }

        bool SkipIfOp(char c)
        {
            if (NextIs(TokenType.operation) && currentToken.lexeme[0] == c)
            {
                Skip();
                return true;
            }
            return false;
        }

        bool NextIs(TokenType type)
        {
            return currentToken.type == type;
        }

        public ProgramNode StartParse()
        {
            while (true)
            {
                if ((currentToken).type == TokenType.variable)
                {
                    if (tokenEnumerator.Current.lexeme == "if")
                    {
                        code.Add(ParseIf());
                    }
                    else if (tokenEnumerator.Current.lexeme == "set")
                    {
                        code.Add(ParseSet());
                    }
                    else if (tokenEnumerator.Current.lexeme == "pop")
                    {
                        code.Add(ParsePop());
                    }
                    else if (tokenEnumerator.Current.lexeme == "call")
                    {
                        code.Add(ParseCall());
                    }
                    else if (tokenEnumerator.Current.lexeme == "goto")
                    {
                        code.Add(ParseGoto());
                    }
                    else if (tokenEnumerator.Current.lexeme[tokenEnumerator.Current.lexeme.Length - 1] == ':')
                    {
                        code.Add(ParseLabel());
                        
                    }
                    else
                    {
                        code.Add(new Variable(ParseIdentifier()));
                    }
                }
                else if ((currentToken).type == TokenType.number)
                {
                    code.Add(ParseNumber());
                }
                else if ((currentToken).type == TokenType.operation)
                {
                    code.Add(ParseOperation());
                }
                else if ((currentToken).type == TokenType.eof)
                {
                    break;
                }
            }
            return new ProgramNode(labels, code);
        }
        
        public Set ParseSet()
        {
            //10 set max;
            //0 set i
            Set res = new Set();
            Skip("set");
            res.variableName = ParseIdentifier();
            return res;
        }

        public Pop ParsePop()
        {
            Pop res = new Pop();
            Skip("pop");
            return res;
        }

        public Label ParseLabel()
        {
            //dd:
            Label res = new Label();
            res.name = tokenEnumerator.Current.lexeme;
            //res.pos = Lexer.pos;
            labels.Add(res.name, code.Count);
            Skip();
            return res;
        }

        public If ParseIf()
        {
            If res = new If();
            Skip("if");
            res.labelName = ParseIdentifier();
            return res;
        }

        public Goto ParseGoto()
        {
            Goto res = new Goto();
            Skip("goto");
            res.label = ParseIdentifier();
            return res;
        }

        public Call ParseCall()
        {
            Call res = new Call();
            Skip("call");
            return res;
        }

        Node ParseNumber()
        {
            if (NextIs(TokenType.number))
            {
                var node = new Number(currentToken.lexeme);
                Skip();
                return node;
            }
            throw Error("Ожидали number");
        }

        string ParseIdentifier()
        {
            if (NextIs(TokenType.variable))
            {
                var data = currentToken.lexeme;
                Skip();
                return data;
            }
            throw Error("Ожидали string");
        }

        Node ParseOperation()
        {
            if (NextIs(TokenType.operation))
            {
                var node = new BinOP(currentToken.lexeme);
               
                Skip();
                return node;
            }
            throw Error("Ожидали operation");
        }

        public Dictionary<string, int> Get()
        {
            return labels;
        }

        #region dispose

        bool disposed;

        ~Parser()
        {
            if (!disposed)
            {
                throw new InvalidOperationException("Dispose не был вызван");
            }
        }

        public void Dispose()
        {
            if (disposed)
            {
                throw new InvalidOperationException("Повторный вызов Dispose");
            }
            tokenEnumerator.Dispose();
            disposed = true;
        }

        #endregion
    }
}