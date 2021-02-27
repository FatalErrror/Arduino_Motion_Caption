using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(PPTX))]

public class PPTXEditor : Editor
{

    PPTX presentation;
    static bool showTileEditor = false;
    static float iconSize = 65f;

    public void OnEnable()
    {
        presentation = (PPTX)target;
        if (presentation.Slides == null)
        {
            presentation.Slides = new Sprite[1];
        }
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        presentation.PPTXName = EditorGUILayout.TextField("Name", presentation.PPTXName);

        int lenth = EditorGUILayout.IntField("Coun of slides in presentation", presentation.Slides.GetLength(0));

        if (lenth != presentation.Slides.Length)
        {
            presentation.Slides = new Sprite[lenth];
        }

        EditorGUILayout.Space(40);
        iconSize = EditorGUILayout.Slider("Icon size", iconSize, 40, 200);
        showTileEditor = EditorGUILayout.Foldout(showTileEditor, "Slides Editor");

        if (showTileEditor)
        {
            //EditorGUILayout.
            for (int l = 0; l < lenth; l++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.SelectableLabel(""+l, GUILayout.Width(30), GUILayout.Height(iconSize));
                presentation.Slides[l] = (Sprite)EditorGUILayout.ObjectField(presentation.Slides[l], typeof(Sprite), false, GUILayout.Width(iconSize), GUILayout.Height(iconSize));
                EditorGUILayout.EndHorizontal();
            }
        }
        bool somethingChanged = EditorGUI.EndChangeCheck();
        if (somethingChanged)
        {
            EditorUtility.SetDirty(presentation);
        }
        serializedObject.ApplyModifiedProperties();
    }

}
