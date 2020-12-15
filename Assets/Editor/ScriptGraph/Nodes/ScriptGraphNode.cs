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

		public Port inputPort;

		public List<Port> outputPorts = new List<Port>();

		protected void CreateInputPort(string name)
		{
			if (inputPort != null) return;

			inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(Port));
			inputPort.portName = name;
			inputContainer.Add(inputPort);
		}

		protected void AddOutputPort(string name)
		{
			var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Port));
			outputPort.portName = name;
			outputPort.userData = outputPorts.Count;
			outputContainer.Add(outputPort);
			outputPorts.Add(outputPort);
		}
	}
}