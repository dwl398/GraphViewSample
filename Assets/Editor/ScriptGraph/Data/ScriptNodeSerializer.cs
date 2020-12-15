using UnityEngine;
using ScriptGraph.Nodes;
using ScriptGraph.Util;

namespace ScriptGraph.Data
{
	public static class ScriptNodeSerializer
	{
		public static byte[] Serialize(ScriptGraphNode node)
		{
			ByteArrayStream stream = new ByteArrayStream();

			switch (node.Type)
			{
				case NodeType.Root:
					break;
				case NodeType.Message:
					Serialize(node as MessageNode, ref stream);
					break;
				case NodeType.None:
				default:
					{
						Debug.LogError("Invalid Node Type :" + node.Type.ToString());
					}
					break;
			}

			return stream.GetBuffer();
		}

		private static void Serialize(MessageNode node,ref ByteArrayStream stream)
		{
			stream.WriteString(node.text);
		}
	}
}