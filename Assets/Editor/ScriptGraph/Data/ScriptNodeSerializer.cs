using UnityEngine;
using ScriptGraph.Nodes;
using ScriptGraph.Util;

namespace ScriptGraph.Data
{
	public static class ScriptNodeSerializer
	{
		public static ScriptNodeData Serialize(ScriptGraphNode node)
		{
			ScriptNodeData data = new ScriptNodeData();
			data.id = node.id;
			data.type = node.Type;
			data.outIds = node.outIds.ToArray();
			data.rect = node.GetPosition();

			ByteArrayStream stream = new ByteArrayStream();

			switch (node.Type)
			{
				case NodeType.Root:
					break;
				case NodeType.Message:
					Serialize(node as MessageNode, ref stream);
					break;
				case NodeType.Branch:
					Serialize(node as BranchNode, ref stream);
					break;
				case NodeType.None:
				default:
					{
						Debug.LogError("Invalid Node Type :" + node.Type.ToString());
					}
					break;
			}

			data.serialData = stream.GetBuffer();

			return data;
		}

		private static void Serialize(MessageNode node,ref ByteArrayStream stream)
		{
			stream.WriteString(node.text);
		}

		private static void Serialize(BranchNode node, ref ByteArrayStream stream)
		{
			stream.WriteBool(node.flag);
		}
	}
}