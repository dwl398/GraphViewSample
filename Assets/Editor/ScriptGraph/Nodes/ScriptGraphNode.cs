using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace ScriptGraph.Nodes
{
	public class ScriptGraphNode : Node
	{
		public int id;

		protected NodeType nodeType;

		public NodeType Type => nodeType;

		public List<int> outIds = new List<int>();
	}
}