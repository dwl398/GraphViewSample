using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

using ScriptGraph.Nodes;

namespace ScriptGraph.Window
{
	public class ScriptGraphView : GraphView
	{
		public ScriptGraphView() : base()
		{
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
			searchWindowProvider.Init(this);
			this.nodeCreationRequest += context =>
			{
				SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindowProvider);
			};
		}
	}
}
