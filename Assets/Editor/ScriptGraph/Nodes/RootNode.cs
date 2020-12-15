using UnityEditor.Experimental.GraphView;

namespace ScriptGraph.Nodes
{
	public class RootNode : ScriptGraphNode
	{
		public RootNode()
		{
			nodeType = NodeType.Root;

			this.title = "Root";

			var outputOort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Port));
			outputOort.portName = "Out";
			outputContainer.Add(outputOort);
		}
	}
}