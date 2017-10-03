using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assambler
{
    interface Node
    {
        string ToDebugString();
        string ToFormattedString();
    }

    class ProgramNode
    {
        public  Dictionary<string, int> labels = new Dictionary<string, int>();
        public  List<Node> code = new List<Node>();

        public ProgramNode(Dictionary<string, int> stats, List<Node> _code)
        {
            labels = stats;
            code = _code;
        }
        public override string ToString()
        {
            return string.Join(" ", labels);
        }
    }

}
