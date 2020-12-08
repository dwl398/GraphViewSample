using UnityEngine;
using UnityEditor;

namespace ScriptGraph.Window
{
	public class ScriptGraphWindow : EditorWindow
	{
		[MenuItem("Tool/ScriptGraph")]
		public static void Open()
		{
			ScriptGraphWindow window = GetWindow<ScriptGraphWindow>();
			window.Show();

			window.titleContent = new GUIContent("ScriptGraph");
		}

		private void OnEnable()
		{
			var scriptGraph = new ScriptGraphView(this);
			this.rootVisualElement.Add(scriptGraph);
		}
	}
}