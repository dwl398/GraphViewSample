using ScriptGraph.Data;

namespace ScriptGraph.Nodes
{
	public class RootNode : ScriptGraphNode
	{
		public RootNode()
		{
			nodeType = NodeType.Root;

			this.title = "Root";
		}
	}
}