using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using ScriptGraph.Nodes;

namespace ScriptGraph.Window
{
	public class ScriptGraphSearchWindowProvider : ScriptableObject, ISearchWindowProvider
	{
		/// <summary>
		/// ウィンドウ
		/// </summary>
		private ScriptGraphWindow _window;

		/// <summary>
		/// グラフビュー
		/// </summary>
		private ScriptGraphView _graphView;

		private Action<ScriptGraphNode> onCreated;

		public void Init(ScriptGraphView scriptGraphView, ScriptGraphWindow scriptGraphWindow, Action<ScriptGraphNode> onCreated)
		{
			_graphView = scriptGraphView;
			_window = scriptGraphWindow;
			this.onCreated = onCreated;
		}

		public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
		{
			var entries = new List<SearchTreeEntry>();
			entries.Add(new SearchTreeGroupEntry(new GUIContent("Create Node")));

			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (var type in assembly.GetTypes())
				{
					if (type.IsClass == false) continue;

					if (type.IsAbstract) continue;

					if (type.IsSubclassOf(typeof(ScriptGraphNode)) == false) continue;

					entries.Add(new SearchTreeEntry(new GUIContent(type.Name)) { level = 1, userData = type });
				}
			}

			return entries;
		}

		public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
		{
			var type = SearchTreeEntry.userData as Type;
			var node = Activator.CreateInstance(type) as Node;

			var worldMousePosition = _window.rootVisualElement.ChangeCoordinatesTo(_window.rootVisualElement.parent, context.screenMousePosition - _window.position.position);
			var localMousePosition = _graphView.contentViewContainer.WorldToLocal(worldMousePosition);

			node.SetPosition(new Rect(localMousePosition, new Vector2(100, 100)));

			_graphView.AddElement(node);

			onCreated?.Invoke(node as ScriptGraphNode);

			return true;
		}
	}
}