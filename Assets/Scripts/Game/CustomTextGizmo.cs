using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace Game.ScreenDebugs
{
    public class CustomTextGizmo : MonoBehaviour
    {
        public static void DrawText(string text, Vector3 worldPos, Color? color = null)
        {
            Handles.BeginGUI();
            var restoreColor = GUI.color;
            if (color.HasValue) GUI.color = color.Value;
            var view = SceneView.currentDrawingSceneView ?? SceneView.lastActiveSceneView;
            if (view == null || view.camera == null)
            {
                GUI.color = restoreColor;
                Handles.EndGUI();
                return;
            }
            Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
            if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
            {
                GUI.color = restoreColor;
                Handles.EndGUI();
                return;
            }
            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text)) * 2;
            GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text);
            GUI.color = restoreColor;
            Handles.EndGUI();
        }
    }
}
#endif