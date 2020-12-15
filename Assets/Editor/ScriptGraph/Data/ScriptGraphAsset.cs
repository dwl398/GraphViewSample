using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScriptGraph.Data
{
	[CreateAssetMenu(fileName = "scriptgraph.asset", menuName = "ScriptGraph Asset")]
	public class ScriptGraphAsset : ScriptableObject
	{
		public List<ScriptNodeData> list = new List<ScriptNodeData>();

		public ScriptNodeData rootNode
		{
			get
			{
				if(list.Count <= 0)
				{
					return null;
				}

				return list[0];
			}
		}

		public int NextId
		{
			get
			{
				int maxId = list.Select(x => x.id).Max();

				return maxId + 1;
			}
		}

		public void AddNode(ScriptNodeData node)
		{
			for (int i = 0; i < list.Count; ++i)
			{
				if (list[i].id == node.id)
				{
					list[i] = node;
					return;
				}
			}

			list.Add(node);
		}

		public void RemoveNode(int id)
		{
			for (int i = 0; i < list.Count; ++i)
			{
				if (list[i].id == id)
				{
					list.RemoveAt(i);
					break;
				}
			}
		}

		public void UpdateNode(ScriptNodeData node)
		{
			for (int i = 0; i < list.Count; ++i)
			{
				if (list[i].id == node.id)
				{
					list[i] = node;
					break;
				}
			}
		}

		public bool Contains(int id)
		{
			return list.Exists(t => t.id == id);
		}
	}
}
