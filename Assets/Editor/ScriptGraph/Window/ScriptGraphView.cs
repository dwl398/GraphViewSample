using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using ScriptGraph.Nodes;
using ScriptGraph.Data;

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
				rootNode.id = 1;

				this.AddElement(rootNode);

				OnCreatedNode(rootNode);
			}
			else
			{
				Load(_window.scriptGraphAsset);
			}

			Init();
		}

		private void Load(ScriptGraphAsset asset)
		{
			foreach(var data in asset.list)
			{
				this.AddElement(ScriptNodeDeserializer.Deserialze(data));
			}

			// TODO:エッジ
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

			this.graphViewChanged += OnGraphViewChanged;

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
			_window.scriptGraphAsset.AddNode(ScriptNodeSerializer.Serialize(node));
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

					_window.scriptGraphAsset.UpdateNode(ScriptNodeSerializer.Serialize(node));
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

					if (outputNode.outIds.Contains(inputNode.id))
					{
						for (int i = 0; i < outputNode.outIds.Count; ++i)
						{
							if (outputNode.outIds[i] == inputNode.id)
							{
								outputNode.outIds[i] = 0;
								break;
							}
						}

						_window.scriptGraphAsset.UpdateNode(ScriptNodeSerializer.Serialize(outputNode));
					}
				}
			}

			foreach (var element in removedElements)
			{
				if (element is ScriptGraphNode)
				{
					var node = element as ScriptGraphNode;

					_window.scriptGraphAsset.RemoveNode(node.id);
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

				if (outputNode.outIds.Contains(inputNode.id) == false)
				{
					if (outputNode.outIds.Count <= edge.output.tabIndex)
					{
						outputNode.outIds.Insert(edge.output.tabIndex, inputNode.id);
					}
					else
					{
						outputNode.outIds[edge.output.tabIndex] = inputNode.id;
					}

					_window.scriptGraphAsset.UpdateNode(ScriptNodeSerializer.Serialize(outputNode));
				}
			}
		}


	}
}
