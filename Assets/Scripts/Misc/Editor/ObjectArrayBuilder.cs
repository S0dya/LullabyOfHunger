using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectArray))]
public class ObjectArrayBuilder : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        ObjectArray objectArray = (ObjectArray)target;

        if (GUILayout.Button("Create array"))
        {
            while (objectArray.transform.childCount > 0) foreach (Transform transf in objectArray.transform) DestroyImmediate(transf.gameObject);

            CreateArray(objectArray.Prefab, objectArray.Direction, objectArray.Amount, objectArray.transform);
        }
        
        serializedObject.ApplyModifiedProperties();
    }

    void CreateArray(GameObject prefab, Vector3 direction, int n, Transform parent)
    {
        Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>();
        Bounds bounds = new Bounds();
        foreach (Renderer renderer in renderers) bounds.Encapsulate(renderer.bounds);
        var size = bounds.size;
        
        var scaledDirecton = new Vector3(direction.x * size.x, direction.y * size.y, direction.z * size.z);
        var rotation = prefab.transform.rotation;
        
        for (int i = 0; i < n; i++) Instantiate(prefab, scaledDirecton * i + prefab.transform.position, rotation, parent);
    }
}