using UnityEditor;
using UnityEngine;

namespace MeFirstGames.Tools
{
#if UNITY_EDITOR

    [CustomEditor (typeof(ReadMe), true, isFallback = true)]
    public class ReadMeEditor : Editor
    {
        public ReadMe Target => (ReadMe) target;

        private bool _editing;
        private string _editingText;

        // Settings used for editing text area
        private readonly GUILayoutOption[] _editingTextLayoutOptions = new GUILayoutOption[] {
                GUILayout.Height(200.0f)
            };

        private void OnEnable()
        {
            _editing = false;
        }

        public override void OnInspectorGUI()
        {
            if (_editing)
            {
                //GUILayout.Label(_editingText);
                GUILayout.Space(10);
                
                _editingText = EditorGUILayout.TextArea(_editingText, _editingTextLayoutOptions);

                if (!Target.Description.Equals(_editingText))
                {
                    Target.Description = _editingText;
                    EditorUtility.SetDirty(Target);
                    EditorUtility.SetDirty(target);
                }

                // Save our changes
                GUILayout.Space(20);
                if (GUILayout.Button("DONE"))
                {
                    _editing = false;
                }
            }
            else
            {
                GUILayout.Label(Target.Description);    
                GUILayout.Space(20);
                
                // Go into edit mode
                if (GUILayout.Button("EDIT"))
                {
                    _editing = true;
                    _editingText = Target.Description;
                }
            }
            
        }
    }
    
#endif
}