using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace gamesolids
{
    [ExecuteInEditMode]
    [CanEditMultipleObjects]
    public class SelectObjectsEditor : EditorWindow
    {
        //storing dropdown selections
        public string[] aMenu;
        public int aChoice = 0;
        string[] nMenu = new string[2] { "is", "contains" };
        int nChoice = 0;

        public UnityEditor.SerializedProperty replaceWith;

        private GameObject lastObj;
        private static SelectObjectsEditor window;
        // Add menu named "My Window" to the Window menu
        [MenuItem("Tools/Query Select &s")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            window = (SelectObjectsEditor)EditorWindow.GetWindow(typeof(SelectObjectsEditor), false, "Select Options");
            window.Show();
            
        }

        private void OnSelectionChange()
        {
            if (Selection.activeGameObject != lastObj)
            {
                aChoice = 0;
                nChoice = 0;
                SelectObjects.selectedQuery = null;
                SelectObjects.selectedDistance = 10f;
                SelectObjects.selectedAngle = 35f;
                SelectObjects.selectedVector = 15f;
                selectedTags = new List<string>();
                selectedChilds = new List<string>();
                selectedLayers = new List<int>();
            }
            lastObj = Selection.activeGameObject;
        }

        Int64 tflags = 0;
        Int64 lflags = 0;
        Int64 cflags = 0;
        List<string> selectedTags = new List<string>();
        List<string> selectedChilds = new List<string>();
        List<int> selectedLayers = new List<int>();
        string[] tags;
        string[] sTags;
        string[] sLayers;

        void OnGUI()
        {
            if (Selection.activeGameObject == null)
            {
                GUILayout.Label("You must select an object", EditorStyles.boldLabel, GUILayout.Width(400f));

            }
            else
            {
                float labelFixedWidth = 80f;
                float buttonFixedWidth = 80f;

                // region filters

                GUILayout.Space(8f);
                GUILayout.Label("Ignore Flags", EditorStyles.boldLabel);

                // force alignment
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();

                // needs set in OnGui, OnInspectorGUI, etc for the InternalEditorUtility to available
                sTags = UnityEditorInternal.InternalEditorUtility.tags;

                GUILayout.Label("Tags", GUILayout.Width(labelFixedWidth));
                tflags = EditorGUILayout.MaskField( "",  (int)tflags, sTags, GUILayout.MinWidth(100f), GUILayout.ExpandWidth(true));
                selectedTags = new List<string>();
                for (int i = 0; i < sTags.Length; i++)
                {
                    if ((tflags & (1 << i)) == (1 << i))
                    {
                        if (selectedTags.Contains(sTags[i]))
                            selectedTags.Add(sTags[i]);
                    }
                }

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();

                GUILayout.Label("Tag Children", GUILayout.Width(labelFixedWidth));
                cflags = EditorGUILayout.MaskField("", (int)cflags, sTags, GUILayout.Width(100f), GUILayout.ExpandWidth(true));
                selectedChilds = new List<string>();
                for (int i = 0; i < sTags.Length; i++)
                {
                    if ((cflags & (1 << i)) == (1 << i))
                    {
                        if (selectedChilds.Contains(sTags[i]) == false)
                            selectedChilds.Add(sTags[i]);
                    }
                }

                GUILayout.EndHorizontal();
                GUILayout.EndVertical();

                //
                sLayers = UnityEditorInternal.InternalEditorUtility.layers;
                GUILayout.Label("Layers", GUILayout.Width(labelFixedWidth));
                lflags = EditorGUILayout.MaskField( "", (int)lflags, sLayers, GUILayout.MinWidth(100f), GUILayout.ExpandWidth(true));

                for (int i = 0; i < sLayers.Length; i++)
                {
                    if ((lflags & (1 << i)) == (1 << i))
                    {
                        if(selectedLayers.Contains(i) == false)
                            selectedLayers.Add(i);
                    }
                }
                
                GUILayout.EndHorizontal();

                // end region

                // region ByType

                GUILayout.Space(8f);
                GUILayout.Label("Selection ", EditorStyles.boldLabel);
                
                GUILayout.BeginHorizontal();
                GUILayout.Label("Type",  GUILayout.Width(labelFixedWidth));
                
                List<Type> aTypes = SelectObjects.GetCurrentGameObjectTypes();
                aMenu = new string[aTypes.Count];
                for (int i = 0; i < aTypes.Count; i++)
                {
                    aMenu[i] = aTypes[i].ToString();
                }
                aChoice = EditorGUILayout.Popup(aChoice, aMenu);

                if (GUILayout.Button("Select",GUILayout.Width(buttonFixedWidth)))
                {
                    SelectObjects.selectedType = aTypes[aChoice];
                    SelectObjects.GetTypedObjectsInScene(selectedLayers.ToArray(), selectedTags.ToArray(), selectedChilds.ToArray());
                }
                GUILayout.EndHorizontal();

                // end region

                // region ByName

                GUILayout.Space(8f);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Name", GUILayout.Width(labelFixedWidth));
                nChoice = EditorGUILayout.Popup(nChoice, nMenu, GUILayout.Width(buttonFixedWidth));

                if (SelectObjects.selectedQuery == null)
                    SelectObjects.selectedQuery = Selection.activeGameObject.name;

                SelectObjects.selectedQuery = EditorGUILayout.TextField(SelectObjects.selectedQuery, GUILayout.Height(18f));

                if (GUILayout.Button("Select", GUILayout.Width(buttonFixedWidth)))
                {
                    if (nChoice == 0)
                    {
                        SelectObjects.GetNamedObjectsInScene(selectedLayers.ToArray(), selectedTags.ToArray(), selectedChilds.ToArray());
                    }
                    else if (nChoice == 1)
                    {
                        SelectObjects.GetLikeObjectsInScene(selectedLayers.ToArray(), selectedTags.ToArray(), selectedChilds.ToArray());
                    }
                    SceneView.lastActiveSceneView.Repaint();
                }
                GUILayout.EndHorizontal();

                // end region

                // region ByAlignment

                GUILayout.Space(8f);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Angle", GUILayout.Width(labelFixedWidth));

                SelectObjects.selectedAngle = EditorGUILayout.Slider(SelectObjects.selectedAngle, 0f, 360f);

                if (GUILayout.Button("Select", GUILayout.Width(buttonFixedWidth)))
                {
                    SelectObjects.GetAngleObjectsInScene(selectedLayers.ToArray(), selectedTags.ToArray(), selectedChilds.ToArray());
                    SceneView.lastActiveSceneView.Repaint();
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(8f);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Vector", GUILayout.Width(labelFixedWidth));

                SelectObjects.selectedVector = EditorGUILayout.Slider(SelectObjects.selectedVector, 0f, 360f);

                if (GUILayout.Button("Select", GUILayout.Width(buttonFixedWidth)))
                {
                    SelectObjects.GetVectorObjectsInScene(selectedLayers.ToArray(), selectedTags.ToArray(), selectedChilds.ToArray());
                    SceneView.lastActiveSceneView.Repaint();
                }
                GUILayout.EndHorizontal();

                // end region

                // region ByDistance

                GUILayout.Space(8f);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Distance", GUILayout.Width(labelFixedWidth));

                SelectObjects.selectedDistance = EditorGUILayout.Slider(SelectObjects.selectedDistance, 0f, 360f);

                if (GUILayout.Button("Select", GUILayout.Width(buttonFixedWidth)))
                {
                    SelectObjects.GetDistanceObjectsInScene(selectedLayers.ToArray(), selectedTags.ToArray(), selectedChilds.ToArray());
                    SceneView.lastActiveSceneView.Repaint();
                }
                GUILayout.EndHorizontal();

                // end region

                // region replace with prefab

                GUILayout.Space(8f);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Replace with", GUILayout.Width(labelFixedWidth));

                SelectObjects.replaceWith = (UnityEngine.GameObject) EditorGUILayout.ObjectField(SelectObjects.replaceWith,typeof(GameObject),false);

                if (GUILayout.Button("Replace", GUILayout.Width(buttonFixedWidth)))
                {
                    SelectObjects.ReplaceSelection();
                    Debug.Log("Items have been replaced.");
                }
                GUILayout.EndHorizontal();

                // end region

            }
        }
    }
}