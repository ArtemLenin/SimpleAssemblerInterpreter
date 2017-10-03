using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assambler
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = File.ReadAllText(@"../../code.txt");
            Console.WriteLine("Задание 14+15");
            Console.WriteLine("Код на ассемблер:\n" + s);
            Console.WriteLine("—————————————————");
            
            ProgramNode program = Parse(s);
            Interpreter(program);
        }

        public static Dictionary<string, int> ParseLabel(string s)
        {
            var lexer = new Lexer(s);
            var tokens = lexer.GetTokens();
            tokens = RemoveSpaces(tokens);
            using (var parser = new Parser(tokens))
            {
                return parser.Get();
            }
        }

        static IEnumerable<Token> RemoveSpaces(IEnumerable<Token> tokens)
        {
            return tokens.Where(x => x.type != TokenType.whitespase);
        }

        static ProgramNode Parse(string s)
        {
            var lexer = new Lexer(s);
            var tokens = lexer.GetTokens();
            tokens = RemoveSpaces(tokens);
            using (var parser = new Parser(tokens))
            {
                return parser.StartParse();
            }
        }

        static void Interpreter(ProgramNode program)
        {
            Stack<object> stack = new Stack<object>();
            Dictionary<string, object> vars = new Dictionary<string, object>();
            vars["print"] = new PrintFunction();
            for (var index = 0; index < program.code.Count; index++)
            {
                Node s = program.code[index];
                switch (s.GetType().Name)
                {
                    case "Call":
                        int _i = int.Parse(stack.Pop().ToString());
                        ICallable c = (ICallable)stack.Pop();
                        List<object> args = new List<object>();
                        for (int i = 0; i < _i; i++)
                        {
                            args.Add(stack.Pop());
                        }
                        stack.Push(c.Call(args));
                        break;
                    case "Goto":
                        index =  program.labels[((Goto)s).label + ":"];
                        break;
                    case "If":
                        if ((bool)stack.Pop())
                            index = program.labels[((If)s).labelName + ":"];
                        break;
                    case "Set":
                        vars[((Set)program.code[index]).variableName] = stack.Pop();
                        break;
                    case "Pop":
                        stack.Pop();
                        break;
                    case "Number":
                        stack.Push(double.Parse(((Number)s).value));
                        break;
                    case "Variable":
                        stack.Push(vars[((Variable)s).variableName]);
                        break;
                    case "BinOP":
                        double b = (double)stack.Pop();
                        double a = (double)stack.Pop();
                        switch (((BinOP)s).op)
                        {
                            case "+":
                                stack.Push(a + b);
                                break;
                            case "-":
                                stack.Push(a - b);
                                break;
                            case "*":
                                stack.Push(a * b);
                                break;
                            case "/":
                                stack.Push(a / b);
                                break;
                            case ">=":
                                stack.Push(a >= b ? true : false);
                                break;
                            case "<=":
                                stack.Push(a <= b ? true : false);
                                break;
                            case "==":
                                stack.Push(a == b ? true : false);
                                break;
                        }
                        break;
                }
            }
        }
    }

    interface ICallable
    {
        object Call(IReadOnlyList<object> args);
    }

    class PrintFunction : ICallable
    {
        public object Call(IReadOnlyList<object> args)
        {
            var first = true;
            foreach (object obj in args)
            {
                if (!first)
                {
                    Console.Write(" ");
                }
                first = false;
                Console.Write(ValueUtils.ValueToString(obj));
            }
            Console.WriteLine();
            return null;
        }
        public override string ToString()
        {
            return "print";
        }
    }

    static class ValueUtils
    {
        public static string ValueToString(object value)
        {
            if (value == null)
            {
                return "null";
            }
            if (value is int)
            {
                return (value).ToString();
            }
            if (value is double)
            {
                return (value).ToString();
            }
            if (value is bool)
            {
                return (bool)value ? "true" : "false";
            }
            if (value is ICallable)
            {
                return value.ToString();
            }
            throw new NotSupportedException($"неподдерживаемый тип значения {value.GetType()}");
        }
    }
}