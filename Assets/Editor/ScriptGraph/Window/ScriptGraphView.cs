using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace ScriptGraph.Window
{
	public class ScriptGraphView : GraphView
	{
		public ScriptGraphView() : base()
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
		}
	}
}
