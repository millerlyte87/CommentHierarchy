using UnityEditor;
using UnityEngine;

namespace MeFirstGames.Tools
{
    #if UNITY_EDITOR
    
    [InitializeOnLoad]
    [ExecuteInEditMode]
    public class CommentHierarchy : MonoBehaviour
    {
        // Perfect amount of offset to perfectly align on previous text
        private static readonly Vector2 MagicOffset = new Vector2(18, 0f);

        // Editor only font
        private static readonly Color EditorOnlyFontColorActive = new Color(0.03f, 0.86f, 0f);
        private static readonly Color EditorOnlyFontColorInactive = new Color(0.03f, 0.53f, 0f);
        private static readonly Color EditorOnlyPrefabFontColorActive =  new Color(0f, 0.86f, 0.39f);
        private static readonly Color EditorOnlyPrefabFontColorInactive =  new Color(0f, 0.62f, 0.31f);
        
        private const string EDITOR_ONLY = "EditorOnly";

        static CommentHierarchy()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
            EditorApplication.update += OnUpdate;
        }

        private static void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceID);
            if (obj != null)
            {
                var gameObject = obj as GameObject;
                if (gameObject != null)
                {
                    // Highlight Editor only
                    if (gameObject.tag.Equals(EDITOR_ONLY))
                    {
                        Color textColor;
                        if (PrefabUtility.IsPartOfAnyPrefab(gameObject))
                        {
                            textColor = gameObject.activeInHierarchy
                                ? EditorOnlyPrefabFontColorActive
                                : EditorOnlyPrefabFontColorInactive;
                        }
                        else
                        {
                            textColor = gameObject.activeInHierarchy
                                ? EditorOnlyFontColorActive
                                : EditorOnlyFontColorInactive;
                        }
                        
                        Rect offsetRect = new Rect(selectionRect.position + MagicOffset, selectionRect.size);
                        EditorGUI.LabelField(offsetRect, gameObject.name, new GUIStyle()
                            {
                                normal = new GUIStyleState() {textColor = textColor},
                                fontStyle = FontStyle.Normal,
                            }
                        );
                    }
                }
            }
        }

        private static void OnUpdate()
        {
            if (Application.isPlaying)
                return;

            foreach (var gameObject in Selection.gameObjects)
            {
                SetToEditorOnly(gameObject);
            }
        }

        private static void SetToEditorOnly(GameObject gameObject)
        {
            // Force comment to be editor only
            if (!gameObject.tag.Equals(EDITOR_ONLY)
                && gameObject.name.Length > 1
                && gameObject.name[0] == '/'
                && gameObject.name[1] == '/')
            {
                gameObject.tag = EDITOR_ONLY;
                EditorUtility.SetDirty(gameObject);
            }
        }
    }
    
    #endif
}
