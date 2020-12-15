using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using ScriptGraph.Data;

namespace ScriptGraph.Nodes
{
	public abstract class ScriptGraphNode : Node
	{
		public int id;

		protected NodeType nodeType;

		public NodeType Type => nodeType;

		public List<int> outIds;

		public abstract void SetData(ScriptNodeData data);
	}
}