using System;
using UnityEngine;
using ScriptGraph.Nodes;

namespace ScriptGraph.Data
{
	[Serializable]
	public class ScriptNodeData
	{
		public int id;

		public NodeType type;

		public Rect rect;

		public int[] outIds;

		public byte[] data;
	}
}
