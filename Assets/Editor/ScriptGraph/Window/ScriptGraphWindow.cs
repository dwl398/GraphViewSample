using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using ScriptGraph.Data;

namespace ScriptGraph.Window
{
	public class ScriptGraphWindow : EditorWindow
	{
		public ScriptGraphAsset scriptGraphAsset;

		private ScriptGraphView graphView;

		public void Open(ScriptGraphAsset scriptGraphAsset)
		{
			this.scriptGraphAsset = scriptGraphAsset;

			graphView = new ScriptGraphView(this);
			this.rootVisualElement.Add(graphView);

			this.Show();
		}

		/// <summary>
		/// Unityで何らかのアセットを開いたときに呼ばれるコールバック
		/// </summary>
		/// <param name="instanceId"></param>
		/// <param name="line"></param>
		/// <returns></returns>
		[OnOpenAsset()]
		public static bool OnOpenAsset(int instanceId, int line)
		{
			if (EditorUtility.InstanceIDToObject(instanceId) is ScriptGraphAsset)
			{
				var scriptGraphAsset = EditorUtility.InstanceIDToObject(instanceId) as ScriptGraphAsset;

				if (HasOpenInstances<ScriptGraphWindow>())
				{
					var window = GetWindow<ScriptGraphWindow>(scriptGraphAsset.name, typeof(SceneView));

					if (window.scriptGraphAsset == null)
					{
						window.Open(scriptGraphAsset);
						return true;
					}

					if (window.scriptGraphAsset.GetInstanceID() == scriptGraphAsset.GetInstanceID())
					{
						window.Focus();
						return false;
					}
					else
					{
						// TODO:切り替え前に保存
						window.Open(scriptGraphAsset);
						window.titleContent.text = scriptGraphAsset.name;
						window.Focus();
						return false;
					}
				}
				else
				{
					// 新規window作成
					var window = GetWindow<ScriptGraphWindow>(scriptGraphAsset.name, typeof(SceneView));

					window.Open(scriptGraphAsset);
					return true;
				}
			}

			return false;
		}

	}
}