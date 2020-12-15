using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using ScriptGraph.Nodes;
using System.Collections.Generic;

namespace ScriptGraph.Window
{
	public class ScriptGraphView : GraphView
	{
		private ScriptGraphWindow _window;

		public ScriptGraphView(ScriptGraphWindow window) : base()
		{
			_window = window;

			if (_window.scriptGraphAsset == null || _window.scriptGraphAsset.rootNode == null)
			{
				var rootNode = new RootNode();

				this.AddElement(rootNode);

				// _window.scriptGraphAsset.AddNode();
			}

			Init();
		}

		/// <summary>
		/// 初期化
		/// </summary>
		private void Init()
		{
			// 親のサイズに合わせてGraphViewのサイズを設定
			this.StretchToParentSize();
			// ズームインアウト
			SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
			// ドラッグで描画範囲を移動
			this.AddManipulator(new ContentDragger());
			// ドラッグで選択した要素を移動
			this.AddManipulator(new SelectionDragger());
			// ドラッグで範囲選択
			this.AddManipulator(new RectangleSelector());
			// ussファイルを読み込んでスタイルに追加
			this.styleSheets.Add(Resources.Load<StyleSheet>("GraphViewBackGround"));
			// 背景を一番後ろに追加
			this.Insert(0, new GridBackground());

			// 右クリックでノード作成するウィンドウ追加
			var searchWindowProvider = ScriptableObject.CreateInstance<ScriptGraphSearchWindowProvider>();
			searchWindowProvider.Init(this, _window, OnCreatedNode);
			this.nodeCreationRequest += context =>
			{
				SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindowProvider);
			};
		}

		public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
		{
			var compatiblePorts = new List<Port>();

			foreach (var port in ports.ToList())
			{
				// 同じノードは繋げない
				if (startPort.node == port.node) continue;

				// Input - Input , Output - Outputは繋げない
				if (startPort.direction == port.direction) continue;

				// ポートタイプが違うものは繋げない
				if (startPort.portType != port.portType) continue;

				compatiblePorts.Add(port);
			}

			return compatiblePorts;
		}

		private void OnCreatedNode(ScriptGraphNode node)
		{

		}

		private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
		{
			OnMovedElements(graphViewChange.movedElements);

			OnRemovedElements(graphViewChange.elementsToRemove);

			OnCreatedEdges(graphViewChange.edgesToCreate);

			return graphViewChange;
		}

		private void OnMovedElements(List<GraphElement> movedElements)
		{
			if (movedElements == null) return;

			foreach (var element in movedElements)
			{
				if (element is ScriptGraphNode)
				{
					var node = element as ScriptGraphNode;
					// _dialogGraphAsset.UpdateNode(node.ToSerializableNode());
				}
			}
		}

		private void OnRemovedElements(List<GraphElement> removedElements)
		{
			if (removedElements == null) return;

			foreach (var element in removedElements)
			{
				if (element is Edge)
				{
					var edge = element as Edge;

					var inputNode = edge.input.node as ScriptGraphNode;
					var outputNode = edge.output.node as ScriptGraphNode;

					//if (outputNode.outIds.Contains(inputNode.Id))
					//{
					//	for (int i = 0; i < outputNode.outIds.Count; ++i)
					//	{
					//		if (outputNode.outIds[i] == inputNode.Id)
					//		{
					//			outputNode.outIds[i] = 0;
					//			break;
					//		}
					//	}

					//	// データ更新
					//	_dialogGraphAsset.UpdateNode(outputNode.ToSerializableNode());
					//}
				}
			}

			foreach (var element in removedElements)
			{
				if (element is ScriptGraphNode)
				{
					var node = element as ScriptGraphNode;
					//_dialogGraphAsset.RemoveById(node.Id);
				}
			}
		}

		/// <summary>
		/// エッジ作成時
		/// </summary>
		/// <param name="edges"></param>
		private void OnCreatedEdges(List<Edge> edges)
		{
			if (edges == null) return;

			foreach (var edge in edges)
			{
				var inputNode = edge.input.node as ScriptGraphNode;
				var outputNode = edge.output.node as ScriptGraphNode;

				// 重複判定
				if (outputNode.outIds.Contains(inputNode.id) == false)
				{
					// データがまだない
					if (outputNode.outIds.Count <= edge.output.tabIndex)
					{
						outputNode.outIds.Insert(edge.output.tabIndex, inputNode.id);
					}
					else
					{
						// あるなら更新
						outputNode.outIds[edge.output.tabIndex] = inputNode.id;
					}
					// データ更新
					// _dialogGraphAsset.UpdateNode(outputNode.ToSerializableNode());
				}
			}
		}


	}
}
