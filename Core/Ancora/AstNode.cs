using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancora
{
    public class AstNode
    {
        public string NodeType = "unknown";
        public Object Value = null;
        public List<AstNode> Children = new List<AstNode>();
    }
}
