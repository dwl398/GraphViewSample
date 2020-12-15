using UnityEngine;
using ScriptGraph.Nodes;
using ScriptGraph.Util;

namespace ScriptGraph.Data
{
	public static class ScriptNodeDeserializer
	{
		public static ScriptGraphNode Deserialze(ScriptNodeData data)
		{
			ScriptGraphNode node = CreateNode(data);

			if(node == null)
			{
				Debug.LogError("node is null. type = " + data.type.ToString());
				return null;
			}

			return node;
		}

		private static ScriptGraphNode CreateNode(ScriptNodeData data)
		{
			ScriptGraphNode node;
			ByteArrayStream stream = new ByteArrayStream(data.serialData);
			switch (data.type)
			{
				case NodeType.Root:
					{
						node = new RootNode();
					}
					break;
				case NodeType.Message:
					{
						node = new MessageNode();
						var temp = node as MessageNode;
						temp.text = stream.ReadString();
					}
					break;
				default:
					node = null;
					break;
			}

			SetCommonData(node, data);

			return node;
		}

		private static void SetCommonData(ScriptGraphNode node, ScriptNodeData data)
		{
			node.id = data.id;
			node.outIds.AddRange(data.outIds);
			node.SetPosition(data.rect);
		}
	}
}