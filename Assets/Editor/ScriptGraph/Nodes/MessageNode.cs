using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using ScriptGraph.Data;

namespace ScriptGraph.Nodes
{
	public class MessageNode : ScriptGraphNode
	{
		private TextField textField;

		public MessageNode()
		{
			nodeType = NodeType.Message;

			// ノードのタイトル設定
			this.title = "Message";

			// ポート（後述）を作成
			var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(Port));
			inputPort.portName = "In";
			inputContainer.Add(inputPort);

			var outputOort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Port));
			outputOort.portName = "Out";
			outputContainer.Add(outputOort);

			// メイン部分に入力欄追加
			textField = new TextField();
			// 複数行対応
			textField.multiline = true;
			// 日本語入力対応
			textField.RegisterCallback<FocusInEvent>(evt => { Input.imeCompositionMode = IMECompositionMode.On; });
			textField.RegisterCallback<FocusOutEvent>(evt => { Input.imeCompositionMode = IMECompositionMode.Auto; });

			this.mainContainer.Add(textField);
		}

		public override void SetData(ScriptNodeData data)
		{

		}
	}
}