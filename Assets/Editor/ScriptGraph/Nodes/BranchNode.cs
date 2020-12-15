using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

namespace ScriptGraph.Nodes
{
	public class BranchNode : ScriptGraphNode
	{
		public enum Flag
		{
			False = 0,

			True = 1,
		}

		private EnumField enumField;

		public bool flag { get { return ((Flag)enumField.value) == Flag.True; } set { enumField.SetValueWithoutNotify(value ? Flag.True : Flag.False); } }

		public BranchNode()
		{
			nodeType = NodeType.Branch;

			this.title = "Branch";

			CreateInputPort("In");

			AddOutputPort("true");

			AddOutputPort("false");

			enumField = new EnumField(Flag.False);
			contentContainer.Add(enumField);
		}
	}
}

