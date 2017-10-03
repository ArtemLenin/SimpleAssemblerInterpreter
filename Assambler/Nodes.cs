using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assambler
{
    class Pop : Node
    {
        public string ToDebugString()
        {
            string s = string.Format("Pop");
            return s;
        }

        public string ToFormattedString()
        {
            string s = string.Format("Pop");
            return s;
        }

        public void PopVoid(int _i, Stack stack)
        {
            for (int i = 0; i < _i; i++)
            {
                stack.Pop();
            }
        }
    }

    class Set : Node
    {
        public string variableName;
        
        public string ToDebugString()
        {
            return string.Format("set({0});", variableName);
        }

        public string ToFormattedString()
        {
            return string.Format("set {0}", variableName);
        }

        public void SetVoid(int _i, Stack stack)
        {
            for (int i = 0; i < _i; i++)
            {
                stack.Pop();
            }
        }
    }

    class If : Node
    {
        public string labelName;

        public string ToDebugString()
        {
            return string.Format("if)");
        }

        public string ToFormattedString()
        {
            return string.Format("if");
        }
    }
        
    class Goto : Node
    {
        public string label;

        public string ToDebugString()
        {
            return string.Format("goto({0})", label);
        } 

        public string ToFormattedString()
        {
            return string.Format("goto {0}", label);
        }

        public void GotoVoid()
        {

        }
    }

    class BinOP : Node
    {
        public string op;

        public BinOP(string _c)
        {
            op = _c;
        }

        public string ToDebugString()
        {
            return string.Format("{0}", op);
        }

        public string ToFormattedString()
        {
            return string.Format("{0}", op);
        }
    }

    class Call : Node
    {
        public string ToDebugString()
        {
            return string.Format("call");
        }

        public string ToFormattedString()
        {
            return string.Format("call");
        }

        
    }

    class Label : Node
    {
        public string name;

        public string ToDebugString()
        {
            return string.Format("{0}:", name);
        }

        public string ToFormattedString()
        {
            return string.Format("{0}:", name);
        }
    }

    class Variable : Node
    {
        public string variableName;

        public Variable(string s)
        {
            variableName = s;
        }

        public string ToDebugString()
        {
            return variableName;
        }

        public string ToFormattedString()
        {
            return variableName;
        }
    }

    class Number : Node
    {
        public string value;

        public Number(string s)
        {
            value = s;
        }

        public string ToDebugString()
        {
            return value;
        }

        public string ToFormattedString()
        {
            return value;
        }
    }
}