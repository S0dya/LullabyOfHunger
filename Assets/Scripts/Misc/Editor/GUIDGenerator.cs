using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GenerateGUID))]
public class GUIDGenerator : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        GenerateGUID generateGUID = (GenerateGUID)target;

        if (GUILayout.Button("Push GUID generation"))
        {
            generateGUID.PushGenerateGUID();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
