using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace ScriptGraph.Nodes
{
	public class MessageNode : ScriptGraphNode
	{
		private TextField textField;

		public string text { get { return textField.text; }set { textField.SetValueWithoutNotify(value); } }

		public MessageNode()
		{
			nodeType = NodeType.Message;

			// ノードのタイトル設定
			this.title = "Message";

			CreateInputPort("In");

			AddOutputPort("Out");

			// メイン部分に入力欄追加
			textField = new TextField();
			// 複数行対応
			textField.multiline = true;
			// 日本語入力対応
			textField.RegisterCallback<FocusInEvent>(evt => { Input.imeCompositionMode = IMECompositionMode.On; });
			textField.RegisterCallback<FocusOutEvent>(evt => { Input.imeCompositionMode = IMECompositionMode.Auto; });

			this.mainContainer.Add(textField);
		}
	}
}