using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using ScriptGraph.Nodes;

namespace ScriptGraph.Window
{
	public class ScriptGraphSearchWindowProvider : ScriptableObject, ISearchWindowProvider
	{
		/// <summary>
		/// グラフビュー
		/// </summary>
		private ScriptGraphView _scriptGraphView;

		public void Init(ScriptGraphView scriptGraphView)
		{
			_scriptGraphView = scriptGraphView;
		}

		public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
		{
			var entries = new List<SearchTreeEntry>();
			entries.Add(new SearchTreeGroupEntry(new GUIContent("Create Node")));

			entries.Add(new SearchTreeEntry(new GUIContent("MessageNode")) { level = 1, userData = typeof(MessageNode)});

			//foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			//{
			//	foreach (var type in assembly.GetTypes())
			//	{
			//		if (type.IsClass && !type.IsAbstract && (type.IsSubclassOf(typeof(DialogEditorNode)))
			//			&& type != typeof(RootNode))
			//		{
			//			entries.Add(new SearchTreeEntry(new GUIContent(type.Name)) { level = 1, userData = type });
			//		}
			//	}
			//}

			return entries;
		}

		public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
		{
			var type = SearchTreeEntry.userData as System.Type;
			var node = Activator.CreateInstance(type) as Node;

			_scriptGraphView.AddElement(node);
			return true;
		}
	}
}