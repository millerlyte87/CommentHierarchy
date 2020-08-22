using System;
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
        private static readonly Color EditorOnlyCommentFontColorActive = new Color(0.03f, 0.86f, 0f);
        private static readonly Color EditorOnlyCommentFontColorInactive = new Color(0.03f, 0.53f, 0f);
        private static readonly Color EditorOnlyCommentPrefabFontColorActive =  new Color(0f, 0.86f, 0.39f);
        private static readonly Color EditorOnlyCommentPrefabFontColorInactive =  new Color(0f, 0.62f, 0.31f);
        private static readonly Color EditorOnlyTodoFontColorActive = new Color(0.92f, 0.45f, 0.93f);
        private static readonly Color EditorOnlyTodoFontColorInactive = new Color(0.74f, 0.38f, 0.75f);
        private static readonly Color EditorOnlyTodoPrefabFontColorActive = new Color(0.68f, 0.37f, 0.69f);
        private static readonly Color EditorOnlyTodoPrefabFontColorInactive = new Color(0.56f, 0.29f, 0.57f);
        
        private const string EDITOR_ONLY = "EditorOnly";

        static CommentHierarchy()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
        }

        private static void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceID);
            if (obj != null)
            {
                var gameObject = obj as GameObject;
                if (gameObject != null)
                {
                    var name = gameObject.name;

                    if (HasPrefix(name, "TODO")
                        || HasPrefix(name, "//TODO"))
                    {
                        DrawLabelField(gameObject,
                            selectionRect,
                            EditorOnlyTodoFontColorActive,
                            EditorOnlyTodoFontColorInactive,
                            EditorOnlyTodoPrefabFontColorActive,
                            EditorOnlyTodoPrefabFontColorInactive);
                    }
                    else if (HasPrefix(name, "//"))
                    {
                        DrawLabelField(gameObject,
                            selectionRect,
                            EditorOnlyCommentFontColorActive,
                            EditorOnlyCommentFontColorInactive,
                            EditorOnlyCommentPrefabFontColorActive,
                            EditorOnlyCommentPrefabFontColorInactive);
                    }
                }
            }
        }
        
        #region Helper Methods

        /// <summary>
        /// Draws a label with a color scheme
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="selectionRect"></param>
        /// <param name="activeColor"></param>
        /// <param name="inactiveColor"></param>
        /// <param name="activePrefabColor"></param>
        /// <param name="inactivePrefabColor"></param>
        private static void DrawLabelField(GameObject gameObject,
            Rect selectionRect,
            Color activeColor,
            Color inactiveColor,
            Color activePrefabColor,
            Color inactivePrefabColor)
        {
            // Try to set the editor 
            SetToEditorOnly(gameObject);
                
            if (gameObject.tag.Equals(EDITOR_ONLY))
            {
                Color textColor;
                if (PrefabUtility.IsPartOfAnyPrefab(gameObject))
                {
                    textColor = gameObject.activeInHierarchy
                        ? activePrefabColor
                        : inactivePrefabColor;
                }
                else
                {
                    textColor = gameObject.activeInHierarchy
                        ? activeColor
                        : inactiveColor;
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
        
        /// <summary>
        /// Sets a gameobject to the EditorOnly tag
        /// </summary>
        /// <param name="gameObject"></param>
        private static void SetToEditorOnly(GameObject gameObject)
        {
            if (!gameObject.tag.Equals(EDITOR_ONLY))
            {
                gameObject.tag = EDITOR_ONLY;
                EditorUtility.SetDirty(gameObject);
            }
        }

        /// <summary>
        /// Checks to see if a prefix is in a string.
        /// Case insensitive and ignores whitespace.
        /// Does not allocate memory.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private static bool HasPrefix(string text, string prefix)
        {
            var i = 0;
            var j = 0;
            while (i < prefix.Length)
            {
                if (i >= text.Length || j > text.Length)
                    break;
                
                var character = text[j];
                if (!char.IsWhiteSpace(character))
                {
                    var prefixCharacter = prefix[i];
                    if (char.ToLower(character) != char.ToLower(prefixCharacter))
                        break;

                    ++i; // Increment Prefix
                }

                ++j; // Increment text
            }

            return i == prefix.Length;
        }
        
        #endregion
    }
    
    #endif
}
